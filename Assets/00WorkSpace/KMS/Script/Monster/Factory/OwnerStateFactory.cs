using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerStateFactory : DefaultMonsterStateFactory
{
    private IMonsterState ownerIdleState;
    private IMonsterState ownerChaseState;
    private IMonsterState ownerAttackState;
    private IMonsterState searchState;
    private IMonsterState suspiciousState;

    public OwnerStateFactory(BaseMonster owner) : base(owner)
    {
        ownerIdleState = new OwnerIdleState();
        ownerChaseState = new OwnerChaseState();
        ownerAttackState = new OwnerAttackState();
    }

    public override IMonsterState GetStateForPerception(MonsterPerceptionState state)
    {
        return state switch
        {
            MonsterPerceptionState.Idle => ownerIdleState,
            MonsterPerceptionState.Alert => ownerChaseState,
            MonsterPerceptionState.Search => searchState,
            MonsterPerceptionState.Suspicious => suspiciousState,
            _ => base.GetStateForPerception(state)
        };
    }

    public override IMonsterState CreateIdleState() => ownerIdleState;
    public override IMonsterState CreateChaseState() => ownerChaseState;
    public override IMonsterState CreateAttackState() => ownerAttackState;

}

