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

    // ��赵 ��ġ ����
    [SerializeField] protected float alertLevel = 0f;
    public float AlertLevel => alertLevel;

    [SerializeField] protected float alertDecayRate = 5f;

    // ��赵 �ܰ�
    [SerializeField] protected float alertThreshold_Low = 20f;
    [SerializeField] protected float alertThreshold_Medium = 50f;
    [SerializeField] protected float alertThreshold_High = 80f;

    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;

    // �ް��� ��ȭ ������ ��ٿ�
    private float alertCooldownTimer = 0f;
    [SerializeField] private float alertCooldownThreshold = 2f;

    protected bool isDead;
    public bool IsDead => isDead;

    // �˹��� ���� �ۿ�
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
        HandleState(); // �ڽ��� override ����
        UpdateAlert();

        Debug.Log($"[DEBUG] alertLevel: {alertLevel:F1}, InSight: {IsInSight()}, perceptionState: {perceptionState}");
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

    protected abstract void HandleState(); // ���¸ӽ� ���� ������ ���⼭

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget() => target;

    public virtual void Move(Vector3 direction)
    {
        if (rb != null)
        {
            Vector3 targetPosition = rb.position + direction * data.moveSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition); // ���� �ݿ� �̵�
        }
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

        Debug.Log($"[BaseMonster] {data.monsterName} ���� ���� �Ϸ�");
    }

    public bool SetPerceptionState(MonsterPerceptionState newState)
    {
        if (perceptionState == newState)
            return false;

        perceptionState = newState;
        UpdateSightParameters();
        return true;
    }

    private void UpdateSightParameters() //���� ����
    {
        float fovMultiplier = 1f;   //�þ� ����
        float rangeMultiplier = 1f; //Ž������ ����

        switch (perceptionState)
        {
            case MonsterPerceptionState.Idle: //��� ����
                fovMultiplier = 0.95f;
                rangeMultiplier = 0.95f;
                break;
            case MonsterPerceptionState.Search: //Ž�� ����
                fovMultiplier = 1.2f;
                rangeMultiplier = 1.2f;
                break;
            case MonsterPerceptionState.Alert: //��� ����
                fovMultiplier = 1.5f;
                rangeMultiplier = 1.5f;
                break;
            case MonsterPerceptionState.Combat: //���� ����
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

        Vector3 eyePosition = transform.position + Vector3.up * data.eyeHeight;
        Vector3 directionToTarget = (target.position - eyePosition).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        float distanceToTarget = Vector3.Distance(eyePosition, target.position);

        Debug.DrawRay(eyePosition, transform.forward * 5f, Color.red);     // forward �þ�
        Debug.DrawRay(eyePosition, directionToTarget * 5f, Color.green);   // Ÿ�� ����

        // ����� �α� �߰�
        Debug.Log($"[SightCheck] angle: {angle:F1}, FOV: {currentFOV:F1}, distance: {distanceToTarget:F1}, range: {currentDetectionRange:F1}");

        if (angle > currentFOV)
        {
            Debug.Log("[SightCheck] ����: �þ߰� ���");
            return false;
        }

        if (distanceToTarget > currentDetectionRange)
        {
            Debug.Log("[SightCheck] ����: �Ÿ� �ʰ�");
            return false;
        }

        if (Physics.Raycast(eyePosition, directionToTarget, out RaycastHit hit, currentDetectionRange))
        {
            Debug.Log($"[SightCheck] Raycast hit: {hit.transform.name}");

            if (hit.transform == target)
            {
                Debug.Log("[SightCheck] ����: Ÿ�� ���� ������");
                return true;
            }
            else
            {
                Debug.Log("[SightCheck] ����: �߰��� �ٸ� ������Ʈ ������");
            }
        }
        else
        {
            Debug.Log("[SightCheck] ����: Raycast�� �ƹ��͵� ������ ����");
        }

        return false;
    }


    public void IncreaseAlert(float amount)
    {
        alertLevel += amount;
        alertLevel = Mathf.Clamp(alertLevel, 0, 100);
        Debug.Log($"[{name}] alertLevel ���� �� {alertLevel:F1}");
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
                Debug.Log($"[{name}] ��� ���� ����: {perceptionState} �� {newState}");
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

        Debug.Log($"[{name}] ���� ���� �õ� �� {state}");

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

        Debug.Log($"[MonsterSearchState] {name} Ž�� ���� ����");
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (rb == null) return;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log($"[Monster] �˹� ����: {direction}, ��: {force}");

        // ���� ���̶�� ���� �ߴ�
        var monster = GetComponent<BaseMonster>();
        monster?.StateMachine?.ChangeState(new MonsterStaggerState(stunTime));
    }
}