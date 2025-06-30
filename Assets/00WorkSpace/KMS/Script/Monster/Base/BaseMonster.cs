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
    [SerializeField] protected float actionRadius = 10f;
    private Vector3 spawnPoint;

    // ȸ�� ����
    [SerializeField] private float rotationSpeed = 7f;

    // ���� �̵� ����
    private float moveTimer = 0f;
    private Vector3 currentDirection;

    // �б� ����
    public float AlertLevel => perceptionController.GetAlertLevel();
    public float AlertThreshold_Low => alertThreshold_Low;
    public float AlertThreshold_Medium => alertThreshold_Medium;
    public float AlertThreshold_High => alertThreshold_High;
    public Vector3 OriginPosition => originPosition;
    public float ActionRadius => actionRadius;


    protected bool isDead;
    public bool IsDead => isDead;

    // ����
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
        spawnPoint = transform.position; //���� �� ��ġ�� �������� ������ �ൿ�ݰ��� ������


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
            Debug.LogError($"{name}�� stateMachine�� null�Դϴ�.");
            return;
        }

        if (perceptionController == null)
        {
            Debug.LogError($"{name}�� perceptionController�� null�Դϴ�.");
            return;
        }

        stateMachine.Update();
        HandleState();
        perceptionController.Update();

        if (stateMachine.CurrentState is MonsterIdleState) // Idle ���¿���
        {
            HandleWanderMovement(); // ���� �̵� ȣ��
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // �浹 ������ ��꿡 SO �� ���
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

                Debug.Log($"[{name}] ���� �õ�: �Ÿ� {distance}m - ���� ����");
            }
            else
            {
                Debug.LogWarning($"[{name}] ���� ��� IDamageable�� �����ϴ�.");
            }
        }
        else
        {
            Debug.Log($"[{name}] ���� ����: �Ÿ� {distance}m - ���� �ʰ�");
        }
    }

    private bool IsFacingTarget()
    {
        Vector3 toTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, toTarget);
        return dot > 0.7f; // 1�� ����� ���� ����
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        view.PlayMonsterDeathAnimation();
        //DropItem();
        OnDeadEvent?.Invoke();


        stateMachine.ChangeState(new MonsterDeadState()); // ���Ⱑ ������
    }

    protected abstract void HandleState(); // ���¸ӽ� ���� ������ ���⼭

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
            Debug.LogWarning("Rigidbody�� �����ϴ�!");
            return;
        }

        // �ൿ �ݰ� ����
        if (IsOutsideActionRadius())
        {
            //Debug.Log($"[{name}] �ൿ �ݰ� �ʰ� �� �̵� ����");
            return;
        }

        // ȸ��
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // �̵�
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
        Debug.Log($"[SetData] GetHpMultiplier(subType) ȣ��, subType: {subType}, ��ȯ��: {mult}");

        // base��
        float baseHP = data.MaxHP;
        float basePower = data.AttackPower;

        // ���� ���
        float hp = baseHP * typeStat.hpMultiplier * stageStat.GetHpMultiplier(subType);
        float power = basePower * typeStat.attackPowerMultiplier * stageStat.GetAttackMultiplier(subType);

        // ���� ����
        currentHP = Mathf.RoundToInt(hp);
        attackPower = Mathf.RoundToInt(power);
        moveSpeed = data.MoveSpeed * typeStat.moveSpeedMultiplier;

        attackCooldown = data.AttackCooldown;
        detectionRange = data.DetectionRange;
        attackRange = data.AttackRange;
        targetType = data.TargetType;

        originPosition = transform.position;
        UpdateSightParameters();

        Debug.Log($"[SetData:Debug] ����: {data.monsterName}, Ÿ��: {subType}\n" +
                  $"- Base HP: {baseHP}, TypeMult: {typeStat.hpMultiplier}, StageMult: {stageStat.GetHpMultiplier(subType)}\n" +
                  $"=> ���� HP: {hp}\n" +
                  $"- Base ATK: {basePower}, TypeMult: {typeStat.attackPowerMultiplier}, StageMult: {stageStat.GetAttackMultiplier(subType)}\n" +
                  $"=> ���� ATK: {power}");
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
    //                // ������ ������, ������, �̸�, ���� ���� itemSO���� ������ ����
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

    private void UpdateSightParameters() //���� ����
    {
       float fovMultiplier = 1f;   //�þ� ����
       float rangeMultiplier = 1f; //Ž������ ����
   
       switch (perceptionState)
       {
           case MonsterPerceptionState.Idle: //��� ����
                fovMultiplier = 1f;
               rangeMultiplier = 1f;
                break;
           case MonsterPerceptionState.Search: //Ž�� ����
               fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
           case MonsterPerceptionState.Alert: //��� ����
               fovMultiplier = 1f;
                rangeMultiplier = 1f;
                break;
           case MonsterPerceptionState.Combat: //���� ����
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
            Debug.Log($"[{name}] (Perception) Return �迭 ���¿����� ���� ���� ����");
            return;
        }
        Debug.Log($"[{name}] ���� ���� �õ� �� {state}");
        IMonsterState nextState = stateFactory.GetStateForPerception(state);
        stateMachine.ChangeState(nextState);
        Debug.Log($"[MonsterSearchState] {name} Ž�� ���� ����");
    }

    public void ApplyKnockback(Vector3 direction, float knockbackDistance)
    {
        if (rb == null) return;
        Vector3 knockbackPos = transform.position + direction.normalized * knockbackDistance;
        rb.MovePosition(knockbackPos);
        Debug.Log($"[Knockback] {name} �˹� ��ġ: {knockbackPos}");
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

        // �þ� ���� ���� (���� �ڵ�)
        Gizmos.color = Color.yellow;
        Vector3 eyePos = transform.position + Vector3.up * data.EyeHeight;
        Gizmos.DrawWireSphere(eyePos, currentDetectionRange);

        // �ൿ �ݰ� (originPosition ����)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPosition, data.ActionRadius);

        // �� ���� �������� ���� ���� �ð�ȭ
        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -currentFOV / 2, 0) * forward;
        Vector3 rightLimit = Quaternion.Euler(0, currentFOV / 2, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePos, eyePos + leftLimit * currentDetectionRange);
        Gizmos.DrawLine(eyePos, eyePos + rightLimit * currentDetectionRange);
    }

    public void TakeDamage(int damage)
    {
        Debug.LogError($"����! ������ {damage}");
    }


}