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
    protected MonsterTargetType targetType;

    protected Transform target;
    protected MonsterStateMachine stateMachine;
    protected MonsterView view;
    public UnityEvent OnDeadEvent;

    protected bool isDead;
    public bool IsDead => isDead;



    protected virtual void Awake()
    {
        stateMachine = new MonsterStateMachine(this);
        view = GetComponent<MonsterView>();
    }

    protected virtual void Update()
    {
        stateMachine.Update();
        HandleState(); // 자식이 override 가능
    }

    public virtual void ReceiveDamage(float amount)
    {
        currentHP -= amount;
        view.PlayHitEffect();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        view.PlayDeathAnimation();
        OnDeadEvent?.Invoke();
    }

    protected abstract void HandleState(); // 상태머신 상태 변경은 여기서

    public virtual bool CanSeePlayer()
    {
        if (target == null) return false;
        float dist = Vector3.Distance(transform.position, target.position);
        return dist < data.detectionRange;
    }

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

        Debug.Log($"[BaseMonster] {data.monsterName} 스탯 설정 완료");
    }
}