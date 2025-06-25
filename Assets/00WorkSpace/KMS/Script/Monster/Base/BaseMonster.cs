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

    //�Ҹ� ����
    [SerializeField] protected float alertLevel = 0f;
    public float AlertLevel => alertLevel;
    [SerializeField] protected float alertDecayRate = 5f;
    [SerializeField] protected float alertThreshold_Search = 10f;
    public float AlertThreshold_Search => alertThreshold_Search;
    [SerializeField] protected float alertThreshold_Alert = 20f;

    //�Ҹ� ������ �޼ӵ� ��ȭ �����
    private float alertCooldownTimer = 0f;
    [SerializeField] private float alertCooldownThreshold = 2f;

    protected bool isDead;
    public bool IsDead => isDead;

    // �˹��� ���� �ۿ�
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float stunTime = 0.5f;



    protected virtual void Awake()
    {
        stateMachine = new MonsterStateMachine(this);
        view = GetComponent<MonsterView>();
    }

    protected virtual void Update()
    {
        stateMachine.Update();
        HandleState(); // �ڽ��� override ����
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

    protected abstract void HandleState(); // ���¸ӽ� ���� ������ ���⼭

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

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        // �þ߰� üũ (currentFOV�� ���ݰ��̹Ƿ� �״�� ��)
        if (angle > currentFOV) return false;

        // �þ� �Ÿ� + ��ֹ� üũ
        Vector3 eyePosition = transform.position + Vector3.up * data.eyeHeight; // �� ��ġ ����
        float distanceToTarget = Vector3.Distance(eyePosition, target.position);

        if (distanceToTarget > currentDetectionRange) return false;

        // Raycast�� ��ֹ� ���� Ȯ��
        if (Physics.Raycast(eyePosition, directionToTarget, out RaycastHit hit, currentDetectionRange))
        {
            if (hit.transform == target)
                return true; // �þ� �ȿ� �ְ�, ������ ��ü ����
        }

        return false;
    }

    public void IncreaseAlert(float amount)
    {
        alertLevel += amount;
        alertLevel = Mathf.Clamp(alertLevel, 0, 100);
    }

    private MonsterPerceptionState EvaluateAlertState()
    {
        if (alertLevel >= alertThreshold_Alert) return MonsterPerceptionState.Alert;
        if (alertLevel >= alertThreshold_Search) return MonsterPerceptionState.Search;
        return MonsterPerceptionState.Idle;
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
            }
        }
        else
        {
            alertCooldownTimer = 0f;
        }
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (rb == null) return;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log($"[Monster] �˹� ����: {direction}, ��: {force}");

        // ���� ���̶�� ���� �ߴ�
        var monster = GetComponent<BaseMonster>();
        monster?.StateMachine?.ChangeState(new StaggerState(stunTime));
    }
}