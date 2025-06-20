using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    [Header("Data")]
    public BaseMonsterData data;

    protected float currentHP;
    protected Transform target;

    protected MonsterStateMachine stateMachine;
    protected MonsterView view;
    public float detectionRange = 10f;


    protected virtual void Awake()
    {
        currentHP = data.maxHP;
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
        view.PlayDeathAnimation();
        // 필요시 디스폰 등 추가
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
}