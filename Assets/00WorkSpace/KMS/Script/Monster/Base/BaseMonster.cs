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

    // ��赵 ��ġ ����
    [SerializeField] protected float alertDecayRate = 5f;
    [SerializeField] private float cooldownTimer = 0f;
    [SerializeField] private float alertCooldownThreshold = 2f;
    private MonsterPerceptionController perceptionController;

    // ��赵 �ܰ�
    [SerializeField] protected float alertThreshold_Low = 20f;
    [SerializeField] protected float alertThreshold_Medium = 50f;
    [SerializeField] protected float alertThreshold_High = 80f;

    // �ൿ �ݰ�
    [SerializeField] protected float actionRadius = 20f; 
    private Vector3 spawnPoint;

    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;

    protected bool isDead;
    public bool IsDead => isDead;

    // �˹��� ���� �ۿ�
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

        perceptionController = new MonsterPerceptionController(
        this,
        alertDecayRate,
        alertCooldownThreshold,
        alertThreshold_Low,
        alertThreshold_Medium,
        alertThreshold_High
        );

        perceptionController.OnPerceptionStateChanged += ChangeStateAccordingToPerception;


        idleState = stateFactory.CreateIdleState();
        suspiciousState = stateFactory.CreateSuspiciousState();
        searchState = stateFactory.CreateSearchState();
        alertState = stateFactory.CreateAlertState();

    }
    protected virtual void Start()
    {
        spawnPoint = transform.position; //���� �� ��ġ�� �������� ������ �ൿ�ݰ��� ������
    }

    protected virtual void Update()
    {
        stateMachine.Update();
        HandleState(); // �ڽ��� override ����
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

        stateMachine.ChangeState(new MonsterDeadState()); // ���Ⱑ ������
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
            Vector3 targetPosition = rb.position + (direction * data.moveSpeed * Time.deltaTime);
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
        return distance <= data.attackRange;
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

    protected bool CheckTargetVisible()
    {
        return sensor.IsTargetVisible(transform, target, currentDetectionRange, currentFOV, data.eyeHeight);
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

    protected virtual void ChangeStateAccordingToPerception(MonsterPerceptionState state)
    {
        Debug.Log($"[{name}] ���� ���� �õ� �� {state}");
        IMonsterState nextState = stateFactory.GetStateForPerception(state);
        stateMachine.ChangeState(nextState);
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