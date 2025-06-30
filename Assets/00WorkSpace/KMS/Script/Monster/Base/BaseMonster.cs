using KMS.Monster.Interface;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseMonster : MonoBehaviour, IDamagable , IKnockbackable
{
    [Header("Data")]
    public BaseMonsterData data;
    private Animator animator;

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
    [SerializeField] protected float actionRadius = 10f;
    private Vector3 spawnPoint;

    // 회전 관련
    [SerializeField] private float rotationSpeed = 7f;

    // 랜덤 이동 관련
    private float moveTimer = 0f;
    private Vector3 currentDirection;

    // 읽기 전용
    public float AlertLevel => perceptionController.GetAlertLevel();
    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;
    public Vector3 OriginPosition => originPosition;
    public float ActionRadius => actionRadius;


    protected bool isDead;
    public bool IsDead => isDead;

    // 기절
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
        HandleState();
        perceptionController.Update();

        if (stateMachine.CurrentState is MonsterIdleState) // Idle 상태에선
        {
            HandleWanderMovement(); // 랜덤 이동 호출
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌 데미지 계산에 SO 값 사용
        var damageable = collision.gameObject.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.TakeDamage(data.CollisionDamage);
        }
    }

    public virtual void ReceiveDamage(float amount, Vector3 direction)
    {
        currentHP -= amount;
        view.PlayMonsterHitEffect();

        float knockbackDistance = CalculateKnockbackDistance();
        ApplyKnockback(direction, knockbackDistance);

        if (currentHP <= 0)
            Die();
    }

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

                Debug.Log($"[{name}] 공격 시도: 거리 {distance}m - 공격 성공");
            }
            else
            {
                Debug.LogWarning($"[{name}] 공격 대상에 IDamageable이 없습니다.");
            }
        }
        else
        {
            Debug.Log($"[{name}] 공격 실패: 거리 {distance}m - 범위 초과");
        }
    }

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
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody가 없습니다!");
            return;
        }

        // 행동 반경 제한
        if (IsOutsideActionRadius())
        {
            //Debug.Log($"[{name}] 행동 반경 초과 → 이동 중지");
            return;
        }

        // 회전
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // 이동
        Vector3 targetPosition = rb.position + (direction * moveSpeed * Time.deltaTime);
        rb.MovePosition(targetPosition);
    }
    private void HandleWanderMovement()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0f)
        {
            float angle = UnityEngine.Random.Range(0f, 360f);
            currentDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;

            moveTimer = UnityEngine.Random.Range(1f, 2f);
        }

        Move(currentDirection);
    }

    public virtual void SetData(BaseMonsterData newData, MonsterTypeStatData typeStat, StageMonsterScalingData stageStat)
    {
        data = newData;
        Debug.Log($"[SetData] stageStat is null? {stageStat == null}");
        MonsterSubType subType = data.monsterSubType;

        float mult = stageStat != null ? stageStat.GetHpMultiplier(subType) : 1f;
        Debug.Log($"[SetData] GetHpMultiplier(subType) 호출, subType: {subType}, 반환값: {mult}");

        // base값
        float baseHP = data.MaxHP;
        float basePower = data.AttackPower;

        // 배율 계산
        float hp = baseHP * typeStat.hpMultiplier * stageStat.GetHpMultiplier(subType);
        float power = basePower * typeStat.attackPowerMultiplier * stageStat.GetAttackMultiplier(subType);

        // 실제 적용
        currentHP = Mathf.RoundToInt(hp);
        attackPower = Mathf.RoundToInt(power);
        moveSpeed = data.MoveSpeed * typeStat.moveSpeedMultiplier;

        attackCooldown = data.AttackCooldown;
        detectionRange = data.DetectionRange;
        attackRange = data.AttackRange;
        targetType = data.TargetType;

        originPosition = transform.position;
        UpdateSightParameters();

        Debug.Log($"[SetData:Debug] 몬스터: {data.monsterName}, 타입: {subType}\n" +
                  $"- Base HP: {baseHP}, TypeMult: {typeStat.hpMultiplier}, StageMult: {stageStat.GetHpMultiplier(subType)}\n" +
                  $"=> 최종 HP: {hp}\n" +
                  $"- Base ATK: {basePower}, TypeMult: {typeStat.attackPowerMultiplier}, StageMult: {stageStat.GetAttackMultiplier(subType)}\n" +
                  $"=> 최종 ATK: {power}");
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
    //                // 아이템 프리팹, 아이콘, 이름, 설명 등은 itemSO에서 가져와 생성
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

    private void UpdateSightParameters() //임의 배정
    {
       float fovMultiplier = 1f;   //시야 배율
       float rangeMultiplier = 1f; //탐지범위 배율
   
       switch (perceptionState)
       {
           case MonsterPerceptionState.Idle: //대기 상태
                fovMultiplier = 1f;
               rangeMultiplier = 1f;
                break;
           case MonsterPerceptionState.Search: //탐색 상태
               fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
           case MonsterPerceptionState.Alert: //경계 상태
               fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
           case MonsterPerceptionState.Combat: //전투 상태
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
            Debug.Log($"[{name}] (Perception) Return 계열 상태에서는 상태 전이 무시");
            return;
        }
        Debug.Log($"[{name}] 상태 전이 시도 → {state}");
        IMonsterState nextState = stateFactory.GetStateForPerception(state);
        stateMachine.ChangeState(nextState);
        Debug.Log($"[MonsterSearchState] {name} 탐색 상태 진입");
    }

    public void ApplyKnockback(Vector3 direction, float knockbackDistance)
    {
        if (rb == null) return;
        Vector3 knockbackPos = transform.position + direction.normalized * knockbackDistance;
        rb.MovePosition(knockbackPos);
        Debug.Log($"[Knockback] {name} 넉백 위치: {knockbackPos}");
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

        // 시야 감지 범위 (기존 코드)
        Gizmos.color = Color.yellow;
        Vector3 eyePos = transform.position + Vector3.up * data.EyeHeight;
        Gizmos.DrawWireSphere(eyePos, currentDetectionRange);

        // 행동 반경 (originPosition 기준)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPosition, data.ActionRadius);

        // 눈 높이 시점에서 방향 각도 시각화
        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -currentFOV / 2, 0) * forward;
        Vector3 rightLimit = Quaternion.Euler(0, currentFOV / 2, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePos, eyePos + leftLimit * currentDetectionRange);
        Gizmos.DrawLine(eyePos, eyePos + rightLimit * currentDetectionRange);
    }

    public void TakeDamage(int damage)
    {
        Debug.LogError($"맞음! 데미지 {damage}");
    }


}