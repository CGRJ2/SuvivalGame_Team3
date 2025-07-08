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


    // 
    public DefaultMonsterStateFactory(BaseMonster owner)
    {
        idleState = new MonsterIdleState(owner);
        suspiciousState = new MonsterSuspiciousState();
        searchState = new MonsterSearchState();
        alertState = new MonsterAlertState();
        attackState = new MonsterAttackState();
    }

    public virtual IMonsterState CreateIdleState() => idleState;
    public virtual IMonsterState CreateSuspiciousState() => suspiciousState;
    public virtual IMonsterState CreateSearchState() => searchState;
    public virtual IMonsterState CreateAlertState() => alertState;
    public virtual IMonsterState CreateAttackState() => attackState;
    public virtual IMonsterState CreateChaseState() => searchState; 
    public virtual IMonsterState CreateStaggerState(float stunTime)
    {
        return new MonsterStaggerState(stunTime); // 매번 새로 생성
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
