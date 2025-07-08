using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class BaseMonster : MonoBehaviour, IDamagable, ISpawnable
{
    // 이 값들을 설정하면 매니저 없이도 프리팹만으로 동작 테스트 가능
    // 초기화 시점: Start()
    // 사용 조건: autoInitialize == true && data == null

    [field: Header("Origin 위치")]
    [field: SerializeField] public Transform OriginTransform { get; set; }

    [Header("몬스터 데이터")]
    public BaseMonsterData data;


    // 공격 준비 중
    public bool isAttackReady;

    protected UnityEngine.AI.NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    public Action DeactiveAction { get; set; }

    [SerializeField] protected float currentHP;
    protected float attackPower;
    protected float attackCooldown;
    protected float detectionRange;
    protected float currentFOV;
    protected float currentDetectionRange;
    protected float attackRange;

    protected MonsterTargetType targetType;
    protected MonsterPerceptionState perceptionState = MonsterPerceptionState.Idle;


    protected Transform target;
    protected MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    [HideInInspector] public MonsterView view;
    public UnityEvent OnDeadEvent;

    public IMonsterState idleState;
    public IMonsterState suspiciousState;
    public IMonsterState searchState;
    public IMonsterState alertState;

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


    // 회전 속도
    [SerializeField] protected float rotationSpeed = 7f;

    [Header("공격 범위 설정")]
    [SerializeField] LayerMask attackableLayerMask;
    [SerializeField] float rayRadius_Attack;
    [SerializeField] Vector3 offset_Attack;
    public IDamagable playerInRange;

    public float RotationSpeed => rotationSpeed;

    private Vector3 currentDirection;

    public float AlertLevel => perceptionController.GetAlertLevel();
    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;

    public float ActionRadius => actionRadius;


    protected bool isDead;
    public bool IsDead => isDead;

    private Rigidbody rb;
    
    public Rigidbody RB => rb;

    [SerializeField] private List<DropTable> dropTableList;


    public IMonsterState GetIdleState() => idleState;
    public IMonsterState GetAlertState() => alertState;
    public IMonsterState GetSearchState() => searchState;
    public IMonsterState GetSuspiciousState() => suspiciousState;

    private IMonsterSensor sensor;

    protected IMonsterStateFactory stateFactory;
    protected virtual void Awake() => Init();

    public virtual void Init()
    {
        rb = GetComponent<Rigidbody>();

        stateMachine = new MonsterStateMachine(this);

        if (stateFactory == null)
            stateFactory = new DefaultMonsterStateFactory(this);

        sensor = new DefaultMonsterSensor();
        view = GetComponent<MonsterView>();
        agent = GetComponent<NavMeshAgent>();
        if (view == null)
            view = GetComponent<MonsterView>();

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

        // 임시
        if (OriginTransform != null) OriginTransform = GameManager.Instance.transform;
    }
    protected virtual void Start()
    {
        // data가 null이면 경고
        if (data == null)
        {
            Debug.LogWarning($"{name} data가 비어있음: 초기화 불가");
            return;
        }

        SetData(data);
        InitTargetByType();

        if (stateFactory == null)
            StateMachine.ChangeState(new MonsterIdleState(this));
        else
            StateMachine.ChangeState(stateFactory.CreateIdleState());
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
    }




    protected virtual void OnDisable()
    {
        DeactiveAction?.Invoke();
        StopAllCoroutines();
    }

    void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.TakeDamage(data.CollisionDamage, transform);
        }
    }

    public void UpdateAttackRange()
    {
        Vector3 origin = view.avatar.transform.position + (view.avatar.transform.forward * offset_Attack.z) + (view.avatar.transform.up * offset_Attack.y) + (view.avatar.transform.right * offset_Attack.x);

        Collider[] cols = Physics.OverlapSphere(origin, rayRadius_Attack, attackableLayerMask);
        cols = cols.Where(c => !c.isTrigger).ToArray();
        List<IDamagable> damagables = new List<IDamagable>();

        foreach (Collider col in cols)
        {
            damagables.Add(col.GetComponent<IDamagable>());
        }

        if (damagables.Count > 0)
            this.playerInRange = damagables[0];
        else this.playerInRange = null;
    }


    // 몬스터 공격 함수
    public void TryAttack()
    {
        if (view != null && view.Animator != null)
            view.Animator.SetFloat("AttackSpeed", data.AttackAnimSpeed);

        view.PlayMonsterAttackAnimation();
    }
    public void ApplyDamage()
    {
        Debug.Log(playerInRange);
        if (playerInRange != null)
        {
            var damageable = target.GetComponent<IDamagable>();
            if (damageable != null)
                damageable.TakeDamage(attackPower, transform);
        }
    }

    // 몬스터 죽음 판정
    protected virtual void Dead()
    {
        if (isDead) return;
        isDead = true;
        view.Animator.SetTrigger("Dead");

        view.PlayMonsterDeathAnimation();
        DropItems();
        OnDeadEvent?.Invoke();

        stateMachine.ChangeState(new MonsterDeadState());
        StartCoroutine(DestroyAfterDelay(data.destroyDelayTime));
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

    public virtual void SetData(BaseMonsterData newData)
    {
        data = newData;
        MonsterSubType subType = data.monsterSubType;

        currentHP = data.MaxHP;
        attackPower = data.AttackPower;
        agent.speed = data.MoveSpeed;

        attackCooldown = data.AttackCooldown;
        detectionRange = data.DetectionRange;
        attackRange = data.AttackRange;
        targetType = data.TargetType;

        UpdateSightParameters();
    }

    public void SetSensor(IMonsterSensor newSensor)
    {
        sensor = newSensor;
    }

    private void DropItems()
    {
        foreach (DropTable dropTable in dropTableList)
        {
            DropInfo dropInfo = dropTable.GetDropItemInfo();
            dropInfo.dropItem.SpawnItem(transform, dropInfo.dropCount);
        }
    }

    public bool SetPerceptionState(MonsterPerceptionState newState)
    {
        if (perceptionState == newState)
            return false;

        perceptionState = newState;
        UpdateSightParameters();
        return true;
    }


    public virtual bool IsOutsideDetectionRadius()
    {
        if (target == null) return true;

        float distance = Vector3.Distance(transform.position, target.position);
        return distance > currentDetectionRange;
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
                PlayerController pc = PlayerManager.Instance.instancePlayer;
                if (pc != null)
                    SetTarget(pc.transform);
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

        IMonsterState nextState = stateFactory.GetStateForPerception(state);
        stateMachine.ChangeState(nextState);
    }




    public void ResetAlert()
    {
        perceptionController.ResetAlert();
    }

   /* protected void OnDrawGizmosSelected()
    {
        Vector3 originPosition = Vector3.zero;
        if (OriginTransform != null) originPosition = OriginTransform.position;

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

        /// 공격 범위
        // Gizmos 색상 지정
        //if (stateMachine.CurrentState = stateFactory.GetAttackState())
        if (isAttackReady)
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // 붉은색 투명
        else
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // 붉은색 투명

        if (view == null) return;
        Vector3 origin_Attack = view.avatar.transform.position + (view.avatar.transform.forward * offset_Attack.z) + (view.avatar.transform.up * offset_Attack.y) + (view.avatar.transform.right * offset_Attack.x);
        Gizmos.DrawSphere(origin_Attack, rayRadius_Attack);
    }*/



    public virtual void TakeDamage(float damage, Transform attackerTransform)
    {
        Debug.Log("공격받음");

        if (data.isInvinvibleMonster) return;

        StartCoroutine(PauseAgent(data.HitStunDuration));

        currentHP -= damage;
        //view.PlayMonsterHitEffect();
        //view.PlayMonsterHitAnimation();

        view.Animator.SetTrigger("Damaged");

        ApplyKnockback(attackerTransform, data.KnockBackedPower);

        if (currentHP <= 0)
            Dead();
    }

    public IEnumerator PauseAgent(float pauseTime)
    {
        agent.isStopped = true;
        rb.isKinematic = false;
        yield return new WaitForSeconds(pauseTime);
        if (agent.isOnNavMesh) agent.isStopped = false;
        rb.isKinematic = true;
    }

    public void ApplyKnockback(Transform transform, float force)
    {
        // 플레이어와 공격자의 방향 벡터를 얻기(dir)     ##주의: 방향 벡터의 Y값을 빼서 평면상의 벡터 방향으로 설정
        Vector3 basicDir = this.transform.position - transform.position;
        Vector3 basicDirToVector2 = new Vector3(basicDir.x, 0, basicDir.z);

        // 공격 방향 + 위로 살짝 합친 벡터를 방향으로 함
        Vector3 finalKnockBackDir = (basicDirToVector2.normalized + (Vector3.up * 0.3f)).normalized;

        GetComponent<Rigidbody>().AddForce(finalKnockBackDir * force, ForceMode.Impulse);
    }

    public virtual void ResetMonsterHP()
    {
        currentHP = data.MaxHP;
        // 필요하면 추가로 회복 이펙트, 로그 등
        //Debug.Log($"[{name}] HP가 최대치로 회복됨");
    }
}