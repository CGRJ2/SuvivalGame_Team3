using KMS.Monster.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class BaseMonster : MonoBehaviour, IDamagable, IKnockbackable
{
    // 이 값들을 설정하면 매니저 없이도 프리팹만으로 동작 테스트 가능
    // 초기화 시점: Start()
    // 사용 조건: autoInitialize == true && data == null
    [Header("자체 초기화용")]
    [SerializeField] private bool autoInitialize = true; // 인스펙터에서 제어 가능
    [SerializeField] private BaseMonsterData defaultDataSO;
    [SerializeField] private MonsterTypeStatData defaultTypeStatSO;
    [SerializeField] private StageMonsterScalingData defaultStageStatSO;

    [Header("Data")]
    public BaseMonsterData data;
    protected Animator animator;

    protected UnityEngine.AI.NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    protected float currentHP;
    protected float moveSpeed;
    protected int attackPower;
    protected float attackCooldown;
    protected float detectionRange;
    protected float currentFOV;
    protected float currentDetectionRange;
    protected float attackRange;
    private Vector3 originPosition;
    protected MonsterTypeStatData typeStat;
    protected MonsterTargetType targetType;
    protected MonsterPerceptionState perceptionState = MonsterPerceptionState.Idle;


    protected Transform target;
    protected MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    protected MonsterView view;
    private MonsterTypeStatData typeStatData;
    public UnityEvent OnDeadEvent;

    private IMonsterState idleState;
    private IMonsterState suspiciousState;
    private IMonsterState searchState;
    private IMonsterState alertState;

    // 경계도 상승
    [SerializeField] protected float alertDecayRate = 5f;
    [SerializeField] private float cooldownTimer = 0f;
    [SerializeField] private float alertCooldownThreshold = 2f;
    protected MonsterPerceptionController perceptionController;

    // 경계도 수치
    [SerializeField] protected float alertThreshold_Low = 20f;
    [SerializeField] protected float alertThreshold_Medium = 50f;
    [SerializeField] protected float alertThreshold_High = 80f;

    // 행동 반경
    [SerializeField] protected float actionRadius = 10f;
    private Vector3 spawnPoint;

    // 회전 속도
    [SerializeField] protected float rotationSpeed = 7f;

    public float RotationSpeed => rotationSpeed;

    private float moveTimer = 0f;
    private Vector3 currentDirection;

    public float AlertLevel => perceptionController.GetAlertLevel();
    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;
    public Vector3 OriginPosition => originPosition;
    public float ActionRadius => actionRadius;


    protected bool isDead;
    public bool IsDead => isDead;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float stunTime = 0.5f;
    public Rigidbody RB => rb;

    public IMonsterState GetIdleState() => idleState;
    public IMonsterState GetAlertState() => alertState;
    public IMonsterState GetSearchState() => searchState;
    public IMonsterState GetSuspiciousState() => suspiciousState;

    private IMonsterSensor sensor;

    protected IMonsterStateFactory stateFactory;
    protected virtual void Awake()
    {

        stateMachine = new MonsterStateMachine(this);
        stateFactory = new DefaultMonsterStateFactory(this);
        sensor = new DefaultMonsterSensor();
        view = GetComponent<MonsterView>();
        agent = GetComponent<NavMeshAgent>();
        if (view == null)
        spawnPoint = transform.position;


        idleState = stateFactory.CreateIdleState();
        suspiciousState = stateFactory.CreateSuspiciousState();
        searchState = stateFactory.CreateSearchState();
        alertState = stateFactory.CreateAlertState();


        perceptionController = new MonsterPerceptionController(
            this,
            alertDecayRate,
            alertCooldownThreshold,
            alertThreshold_Low,
            alertThreshold_Medium,
            alertThreshold_High
        );

        perceptionController.OnPerceptionStateChanged += ChangeStateAccordingToPerception;


        perceptionController.ForceSetState(MonsterPerceptionState.Idle);
    }
    protected virtual void Start()
    {
        if (autoInitialize && !isDead && data == null)
        {
            if (defaultDataSO == null || defaultTypeStatSO == null || defaultStageStatSO == null)
            {
                Debug.LogWarning($"{name} 자동 초기화 실패: 초기화용 SO가 비어 있음");
                return;
            }
            Debug.Log($"[{name}] 자동 초기화 시작");
            SetData(defaultDataSO, defaultTypeStatSO, defaultStageStatSO);
            InitTargetByType();
            StateMachine.ChangeState(stateFactory.CreateIdleState());
        }
    }

    protected virtual void Update()
    {
        if (stateMachine == null)
        {
            return;
        }

        if (perceptionController == null)
        {
            return;
        }

        stateMachine.Update();
        HandleState();
        perceptionController.Update();

        if (stateMachine.CurrentState is MonsterIdleState) // Idle 상태 진입시
        {
            HandleWanderMovement(); //랜덤 이동
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.TakeDamage(data.CollisionDamage);
        }
    }

    //public virtual void ReceiveDamage(float amount, Vector3 direction)
    //{
    //    currentHP -= amount;
    //    view.PlayMonsterHitEffect();
    //
    //    float knockbackDistance = CalculateKnockbackDistance();
    //    ApplyKnockback(direction, knockbackDistance);
    //
    //    if (currentHP <= 0)
    //        Die();
    //}

    public void TryAttack()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange * 0.95f && IsFacingTarget())
        {
            var damageable = target.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackPower);

                if (damageable is IKnockbackable knockbackable)
                {
                    Vector3 direction = (target.position - transform.position).normalized;

                    float knockbackDistance = CalculateKnockbackDistance();
                    knockbackable.ApplyKnockback(direction, knockbackDistance);
                }

                if (view != null && view.Animator != null)
                    view.Animator.SetFloat("AttackSpeed", data.AttackAnimSpeed);

                view?.PlayMonsterAttackAnimation();
            }
        }
    }

    protected abstract void Phase2TryAttack();
    protected abstract void Phase3TryAttack();

    private bool IsFacingTarget()
    {
        Vector3 toTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, toTarget);
        return dot > 0.7f; // 1에 가까울 수록 정면
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        view.PlayMonsterDeathAnimation();
        //DropItem();
        OnDeadEvent?.Invoke();


        stateMachine.ChangeState(new MonsterDeadState());
        StartCoroutine(DestroyAfterDelay(10f));
    }
    private IEnumerator DestroyAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
    protected abstract void HandleState(); // 상태 변환

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget() => target;

    public Vector3 GetSpawnPoint() => spawnPoint;


    //public virtual void Move(Vector3 direction, float customSpeed = -1f)
    //{
    //    float speed = (customSpeed > 0f) ? customSpeed : moveSpeed;
    //
    //    //if (rb == null)
    //    //{
    //    //    
    //    //    return;
    //    //}
    //
    //    
    //    
    //    //if (IsOutsideActionRadius())
    //    //{
    //    //    
    //    //    return;
    //    //}
    //
    //   //회전
    //    //if (direction != Vector3.zero)
    //    //{
    //    //    Quaternion lookRotation = Quaternion.LookRotation(direction);
    //    //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    //    //}
    //    //
    //    //// 이동
    //    //Vector3 targetPosition = rb.position + (direction * speed * Time.deltaTime);
    //    //rb.MovePosition(targetPosition);
    //}
    public virtual void MoveTo(Vector3 destination)
    {
        if (agent == null)
        {
            Debug.LogWarning("NavMeshAgent가 없습니다!");
            return;
        }

        agent.speed = moveSpeed;
        agent.SetDestination(destination);
    }
    private void HandleWanderMovement()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0f)
        {
            // NavMesh 내에서 랜덤 위치 뽑기
            Vector3 randomPoint = originPosition + Random.insideUnitSphere * actionRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, actionRadius, NavMesh.AllAreas))
            {
                MoveTo(hit.position); // 새로운 목적지로 이동
            }

            moveTimer = Random.Range(1f, 4f);
        }
    }

    public virtual void SetData(BaseMonsterData newData, MonsterTypeStatData typeStat, StageMonsterScalingData stageStat)
    {
        data = newData;
        MonsterSubType subType = data.monsterSubType;

        float mult = stageStat != null ? stageStat.GetHpMultiplier(subType) : 1f;

        float baseHP = data.MaxHP;
        float basePower = data.AttackPower;


        float hp = baseHP * typeStat.hpMultiplier * stageStat.GetHpMultiplier(subType);
        float power = basePower * typeStat.attackPowerMultiplier * stageStat.GetAttackMultiplier(subType);

        currentHP = Mathf.RoundToInt(hp);
        attackPower = Mathf.RoundToInt(power);
        moveSpeed = data.MoveSpeed * typeStat.moveSpeedMultiplier;

        attackCooldown = data.AttackCooldown;
        detectionRange = data.DetectionRange;
        attackRange = data.AttackRange;
        targetType = data.TargetType;

        originPosition = transform.position;
        UpdateSightParameters();
    }

    public void SetSensor(IMonsterSensor newSensor)
    {
        sensor = newSensor;
    }

    //private void DropItems()
    //{
    //    if (data.dropTable == null || data.dropTable.Count == 0) return;
    //
    //    foreach (var entry in data.dropTable)
    //    {
    //        if (Random.value <= entry.dropChance)
    //        {
    //            int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
    //            for (int i = 0; i < amount; i++)
    //            {
    //                // 아이템 구현 방식에 따라 수정 필요.
    //                ItemFactory.SpawnItem(entry.itemSO, transform.position);
    //            }
    //        }
    //    }
    //}


    public bool SetPerceptionState(MonsterPerceptionState newState)
    {
        if (perceptionState == newState)
            return false;

        perceptionState = newState;
        UpdateSightParameters();
        return true;
    }

    public virtual bool IsInAttackRange()
    {
        if (target == null) return false;

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;

        float distance = toTarget.magnitude;
        return distance <= data.AttackRange;
    }

    public virtual bool IsOutsideDetectionRadius()
    {
        if (target == null) return true;

        float distance = Vector3.Distance(transform.position, target.position);
        return distance > currentDetectionRange;
    }

    public bool IsOutsideActionRadius()
    {
        return Vector3.Distance(originPosition, transform.position) > actionRadius;
    }

    protected virtual void UpdateSightParameters() //상태에따른 시야/감지범위 조절 기능
    {
        float fovMultiplier = 1f;   // 시야 배율
        float rangeMultiplier = 1f; // 감지 배율

        switch (perceptionState)
        {
            case MonsterPerceptionState.Idle: // 대기 상태
                fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
            case MonsterPerceptionState.Search: // 탐지 상태
                fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
            case MonsterPerceptionState.Alert: // 경계 상태
                fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
            case MonsterPerceptionState.Combat: // 전투 상태
                fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
        }

        currentFOV = data.BaseFOV * fovMultiplier;
        currentDetectionRange = data.DetectionRange * rangeMultiplier;
    }

    protected bool CheckTargetVisible()
    {
        return sensor.IsTargetVisible(transform, target, currentDetectionRange, currentFOV, data.EyeHeight);
    }
    public bool checkTargetVisible => CheckTargetVisible();
    public MonsterPerceptionState GetCurrentPerceptionState()
    {
        return perceptionController.CurrentState;
    }
    public IMonsterStateFactory StateFactory => stateFactory;

    public void IncreaseAlert(float amount)
    {
        perceptionController.IncreaseAlert(amount);
    }
    public virtual IMonsterState CreateAttackState()
    {
        return stateFactory.CreateAttackState();
    }
    public virtual IMonsterState GetAttackState()
    {
        return stateFactory.CreateAttackState();
    }

    protected virtual void InitTargetByType()
    {
        switch (targetType)
        {
            case MonsterTargetType.Player:
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                    SetTarget(player.transform);
                break;

            case MonsterTargetType.Ally:
                GameObject ally = GameObject.FindWithTag("Ally");
                if (ally != null)
                    SetTarget(ally.transform);
                break;

            case MonsterTargetType.None:
                GameObject none = GameObject.FindWithTag("None");
                if (none != null)
                    SetTarget(none.transform);
                break;
        }
    }
    public void DecreaseAlert(float amount)
    {
        perceptionController?.DecreaseAlert(amount);
    }

    protected virtual void ChangeStateAccordingToPerception(MonsterPerceptionState state)
    {
        if (stateMachine.CurrentState is MonsterReturnWaitState ||
            stateMachine.CurrentState is MonsterReturnState)
        {
            return;
        }
        IMonsterState nextState = stateFactory.GetStateForPerception(state);
        stateMachine.ChangeState(nextState);
    }

    public void ApplyKnockback(Vector3 direction, float knockbackDistance)
    {
        if (rb == null) return;
        Vector3 knockbackPos = transform.position + (direction.normalized * knockbackDistance);
        rb.MovePosition(knockbackPos);
    }

    public float CalculateKnockbackDistance()
    {
        return data.KnockbackDistance * typeStat.knockbackDistanceMultiplier;
    }

    public void ResetAlert()
    {
        perceptionController.ResetAlert();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (data == null) return;

        // 감지 반경
        Gizmos.color = Color.yellow;
        Vector3 eyePos = transform.position + (Vector3.up * data.EyeHeight);
        Gizmos.DrawWireSphere(eyePos, currentDetectionRange);

        // 행동 반경 (originPosition 중심)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPosition, data.ActionRadius);

        // 시야 
        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -currentFOV / 2, 0) * forward;
        Vector3 rightLimit = Quaternion.Euler(0, currentFOV / 2, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePos, eyePos + (leftLimit * currentDetectionRange));
        Gizmos.DrawLine(eyePos, eyePos + (rightLimit * currentDetectionRange));
    }

    public virtual void TakeDamage(int damage) // 기존 인터페이스용 TakeDamage
    {
        currentHP -= damage;
        view.PlayMonsterHitEffect();

        if (currentHP <= 0)
            Die();
    }
    public void TakeDamage(int damage, Vector3 direction) // 넉백용 TakeDamage
    {
        currentHP -= damage;
        view.PlayMonsterHitEffect();

        float knockbackDistance = CalculateKnockbackDistance();
        ApplyKnockback(direction, knockbackDistance);

        if (currentHP <= 0)
            Die();
    }
    public virtual void ResetMonsterHP()
    {
        currentHP = data.MaxHP;
        // 필요하면 추가로 회복 이펙트, 로그 등
        Debug.Log($"[{name}] HP가 최대치로 회복됨");
    }
}