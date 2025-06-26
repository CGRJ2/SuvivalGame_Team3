using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseMonster : MonoBehaviour
{
    [Header("Data")]
    public BaseMonsterData data;

    protected float currentHP;
    protected float moveSpeed;
    protected float attackPower;
    protected float attackCooldown;
    protected float detectionRange;
    protected float currentFOV;
    protected float currentDetectionRange;
    protected float attackRange;
    private Vector3 originPosition;
    protected MonsterTargetType targetType;
    protected MonsterPerceptionState perceptionState = MonsterPerceptionState.Idle;

    protected Transform target;
    protected MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    protected MonsterView view;
    public UnityEvent OnDeadEvent;

    private IMonsterState idleState;
    private IMonsterState suspiciousState;
    private IMonsterState searchState;
    private IMonsterState alertState;

    // 경계도 수치 관련
    [SerializeField] protected float alertDecayRate = 5f;
    [SerializeField] private float cooldownTimer = 0f;
    [SerializeField] private float alertCooldownThreshold = 2f;
    private MonsterPerceptionController perceptionController;

    // 경계도 단계
    [SerializeField] protected float alertThreshold_Low = 20f;
    [SerializeField] protected float alertThreshold_Medium = 50f;
    [SerializeField] protected float alertThreshold_High = 80f;

    // 행동 반경
    [SerializeField] protected float actionRadius = 20f;
    private Vector3 spawnPoint;

    public float AlertLevel => perceptionController.GetAlertLevel();
    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;
    

    protected bool isDead;
    public bool IsDead => isDead;

    // 넉백의 물리 작용
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float stunTime = 0.5f;

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
        spawnPoint = transform.position; //스폰 된 위치를 기점으로 몬스터의 행동반경이 정해짐


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
        
    }

    protected virtual void Update()
    {
        if (stateMachine == null)
        {
            Debug.LogError($"{name}의 stateMachine이 null입니다.");
            return;
        }

        if (perceptionController == null)
        {
            Debug.LogError($"{name}의 perceptionController가 null입니다.");
            return;
        }

        stateMachine.Update();
        HandleState(); // 자식이 override 가능
        perceptionController.Update();
    }

    public virtual void ReceiveDamage(float amount)
    {
        currentHP -= amount;
        view.PlayMonsterHitEffect();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        view.PlayMonsterDeathAnimation();
        OnDeadEvent?.Invoke();

        stateMachine.ChangeState(new MonsterDeadState()); // 여기가 진입점
    }

    protected abstract void HandleState(); // 상태머신 상태 변경은 여기서

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget() => target;

    public Vector3 GetSpawnPoint() => spawnPoint;


    public virtual void Move(Vector3 direction)
    {
        Debug.Log($"[Move] 호출됨 - 방향: {direction}");

        if (rb != null)
        {
            Vector3 targetPosition = rb.position + (direction * data.MoveSpeed * Time.deltaTime);
            rb.MovePosition(targetPosition);
        }
        else
        {
            Debug.LogWarning("Rigidbody가 없습니다!");
        }
    }

    public virtual void SetData(BaseMonsterData newData, MonsterTypeStatData typeStat, StageMonsterScalingData stageStat)
    {
        data = newData;

        MonsterSubType subType = data.monsterSubType;

        // base값
        float baseHP = data.MaxHP;
        float basePower = data.AttackPower;

        // 배율 계산
        float hp = baseHP * typeStat.hpMultiplier * stageStat.GetHpMultiplier(subType);
        float power = basePower * typeStat.attackPowerMultiplier * stageStat.GetAttackMultiplier(subType);

        // 실제 적용
        currentHP = hp;
        attackPower = power;
        moveSpeed = data.MoveSpeed * typeStat.moveSpeedMultiplier;

        attackCooldown = data.AttackCooldown;
        detectionRange = data.DetectionRange;
        attackRange = data.AttackRange;
        targetType = data.TargetType;

        originPosition = transform.position;
        UpdateSightParameters();

        Debug.Log($"[BaseMonster] {data.monsterName} 스탯 설정 완료 - HP:{hp} / ATK:{power}");
    }

    public void SetSensor(IMonsterSensor newSensor)
    {
        sensor = newSensor;
    }

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

    public virtual bool IsOutsideActionRadius()
    {
        return Vector3.Distance(transform.position, spawnPoint) > actionRadius;
    }

    private void UpdateSightParameters() //임의 배정
    {
        float fovMultiplier = 1f;   //시야 배율
        float rangeMultiplier = 1f; //탐지범위 배율

        switch (perceptionState)
        {
            case MonsterPerceptionState.Idle: //대기 상태
                fovMultiplier = 0.95f;
                rangeMultiplier = 0.95f;
                break;
            case MonsterPerceptionState.Search: //탐색 상태
                fovMultiplier = 1.2f;
                rangeMultiplier = 1.2f;
                break;
            case MonsterPerceptionState.Alert: //경계 상태
                fovMultiplier = 1.5f;
                rangeMultiplier = 1.5f;
                break;
            case MonsterPerceptionState.Combat: //전투 상태
                fovMultiplier = 0.8f;
                rangeMultiplier = 0.8f;
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

    protected virtual void ChangeStateAccordingToPerception(MonsterPerceptionState state)
    {
        Debug.Log($"[{name}] 상태 전이 시도 → {state}");
        IMonsterState nextState = stateFactory.GetStateForPerception(state);
        stateMachine.ChangeState(nextState);
        Debug.Log($"[MonsterSearchState] {name} 탐색 상태 진입");
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (rb == null) return;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log($"[Monster] 넉백 적용: {direction}, 힘: {force}");

        // 공격 중이라면 상태 중단
        var monster = GetComponent<BaseMonster>();
        monster?.StateMachine?.ChangeState(new MonsterStaggerState(stunTime));
    }

    public void ResetAlert()
    {
        perceptionController.ResetAlert();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (data == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * data.EyeHeight, currentDetectionRange);

        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -currentFOV / 2, 0) * forward;
        Vector3 rightLimit = Quaternion.Euler(0, currentFOV / 2, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * data.EyeHeight, transform.position + leftLimit * currentDetectionRange);
        Gizmos.DrawLine(transform.position + Vector3.up * data.EyeHeight, transform.position + rightLimit * currentDetectionRange);
    }
}