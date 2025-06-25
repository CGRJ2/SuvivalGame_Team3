using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CatMonsterStateFactory : DefaultMonsterStateFactory
{
    private IMonsterState catIdleState;
    private IMonsterState catSearchState;
    private IMonsterState suspiciousState;
    private IMonsterState alertState;

    public CatMonsterStateFactory(BaseMonster cat) : base(cat)
    {
        catIdleState = new CatIdleState();
        catSearchState = new CatSearchState();
    }

    public override IMonsterState GetStateForPerception(MonsterPerceptionState state)
    {
        return state switch
        {
            MonsterPerceptionState.Idle => catIdleState,
            MonsterPerceptionState.Search => catSearchState,
            MonsterPerceptionState.Suspicious => suspiciousState,
            MonsterPerceptionState.Alert => alertState,
            _ => base.GetStateForPerception(state)
        };
    }

    public IMonsterState CreatePacifiedState(float duration)
    {
        return new CatPacifiedState(duration);
    }
}
