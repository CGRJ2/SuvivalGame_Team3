using System.Collections;
using System.Collections.Generic;
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
    protected MonsterTargetType targetType;
    protected MonsterPerceptionState perceptionState = MonsterPerceptionState.Idle;

    protected Transform target;
    protected MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    protected MonsterView view;
    public UnityEvent OnDeadEvent;

    private MonsterIdleState idleState;
    private MonsterSuspiciousState suspiciousState;
    private MonsterSearchState searchState;
    private MonsterAlertState alertState;

    // 경계도 수치 관련
    [SerializeField] protected float alertLevel = 0f;
    public float AlertLevel => alertLevel;

    [SerializeField] protected float alertDecayRate = 5f;

    // 경계도 단계
    [SerializeField] protected float alertThreshold_Low = 20f;
    [SerializeField] protected float alertThreshold_Medium = 50f;
    [SerializeField] protected float alertThreshold_High = 80f;

    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;

    // 급격한 변화 방지용 쿨다운
    private float alertCooldownTimer = 0f;
    [SerializeField] private float alertCooldownThreshold = 2f;

    protected bool isDead;
    public bool IsDead => isDead;

    // 넉백의 물리 작용
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float stunTime = 0.5f;

    public IMonsterState GetIdleState() => idleState;
    public IMonsterState GetAlertState() => alertState;
    public IMonsterState GetSearchState() => searchState;
    public IMonsterState GethsuspiciousStatetate() => suspiciousState;

    protected virtual void Awake()
    {
        stateMachine = new MonsterStateMachine(this);
        view = GetComponent<MonsterView>();

        idleState = new MonsterIdleState();
        suspiciousState = new MonsterSuspiciousState();
        searchState = new MonsterSearchState();
        alertState = new MonsterAlertState();
    }

    protected virtual void Update()
    {
        stateMachine.Update();
        HandleState(); // 자식이 override 가능
        UpdateAlert();
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
    }

    protected abstract void HandleState(); // 상태머신 상태 변경은 여기서

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget() => target;

    public virtual void Move(Vector3 direction)
    {
        transform.position += direction * data.moveSpeed * Time.deltaTime;
    }

    public virtual void SetData(BaseMonsterData newData)
    {
        data = newData;

        currentHP = data.maxHP;
        moveSpeed = data.moveSpeed;
        attackPower = data.attackPower;
        attackCooldown = data.attackCooldown;
        detectionRange = data.detectionRange;
        targetType = data.targetType;

        UpdateSightParameters();

        Debug.Log($"[BaseMonster] {data.monsterName} 스탯 설정 완료");
    }

    public bool SetPerceptionState(MonsterPerceptionState newState)
    {
        if (perceptionState == newState)
            return false;

        perceptionState = newState;
        UpdateSightParameters();
        return true;
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

        currentFOV = data.baseFOV * fovMultiplier;
        currentDetectionRange = data.detectionRange * rangeMultiplier;
    }

    public bool IsInSight()
    {
        if (target == null) return false;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        // 시야각 체크 (currentFOV는 절반각이므로 그대로 비교)
        if (angle > currentFOV) return false;

        // 시야 거리 + 장애물 체크
        Vector3 eyePosition = transform.position + Vector3.up * data.eyeHeight; // 눈 위치 보정
        float distanceToTarget = Vector3.Distance(eyePosition, target.position);

        if (distanceToTarget > currentDetectionRange) return false;

        // Raycast로 장애물 여부 확인
        if (Physics.Raycast(eyePosition, directionToTarget, out RaycastHit hit, currentDetectionRange))
        {
            if (hit.transform == target)
                return true; // 시야 안에 있고, 가리는 물체 없음
        }

        return false;
    }

    public void IncreaseAlert(float amount)
    {
        alertLevel += amount;
        alertLevel = Mathf.Clamp(alertLevel, 0, 100);
    }

    protected MonsterPerceptionState EvaluateAlertState()
    {
        if (alertLevel >= alertThreshold_High)
            return MonsterPerceptionState.Alert;
        if (alertLevel >= alertThreshold_Medium)
            return MonsterPerceptionState.Search;
        if (alertLevel >= alertThreshold_Low)
            return MonsterPerceptionState.Suspicious;
        return MonsterPerceptionState.Idle;
    }
    public MonsterPerceptionState EvaluateCurrentAlertState()
    {
        return EvaluateAlertState();
    }
    protected void UpdateAlert()
    {
        if (alertLevel > 0f)
            alertLevel -= alertDecayRate * Time.deltaTime;

        MonsterPerceptionState newState = EvaluateAlertState();

        if (newState != perceptionState)
        {
            alertCooldownTimer += Time.deltaTime;
            if (alertCooldownTimer >= alertCooldownThreshold)
            {
                SetPerceptionState(newState);
                alertCooldownTimer = 0f;

                ChangeStateAccordingToPerception(newState);
            }
        }
        else
        {
            alertCooldownTimer = 0f;
        }
    }

    protected virtual void ChangeStateAccordingToPerception(MonsterPerceptionState state)
    {
        if (stateMachine == null) return;

        switch (state)
        {
            case MonsterPerceptionState.Idle:
                stateMachine.ChangeState(idleState);
                break;
            case MonsterPerceptionState.Suspicious:
                stateMachine.ChangeState(suspiciousState);
                break;
            case MonsterPerceptionState.Search:
                stateMachine.ChangeState(searchState);
                break;
            case MonsterPerceptionState.Alert:
                stateMachine.ChangeState(alertState);
                break;
        }
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
}