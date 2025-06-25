using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMonsterStateFactory : IMonsterStateFactory
{
    private readonly IMonsterState idleState;
    private readonly IMonsterState suspiciousState;
    private readonly IMonsterState searchState;
    private readonly IMonsterState alertState;
    private readonly IMonsterState attackState;
    private IMonsterState staggerState;

    public DefaultMonsterStateFactory(BaseMonster owner)
    {
        idleState = new MonsterIdleState();
        suspiciousState = new MonsterSuspiciousState();
        searchState = new MonsterSearchState();
        alertState = new MonsterAlertState();
        attackState = new MonsterAttackState();
    }

    public IMonsterState CreateIdleState() => idleState;
    public IMonsterState CreateSuspiciousState() => suspiciousState;
    public IMonsterState CreateSearchState() => searchState;
    public IMonsterState CreateAlertState() => alertState;
    public IMonsterState CreateAttackState() => attackState;
    public IMonsterState CreateStaggerState(float stunTime)
    {
        return new MonsterStaggerState(stunTime); // �Ź� ���� ����
    }

    public virtual IMonsterState GetStateForPerception(MonsterPerceptionState state)
    {
        return state switch
        {
            MonsterPerceptionState.Idle => idleState,
            MonsterPerceptionState.Suspicious => suspiciousState,
            MonsterPerceptionState.Search => searchState,
            MonsterPerceptionState.Alert => alertState,
            _ => idleState
        };
    }
}
