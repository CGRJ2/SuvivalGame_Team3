using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterStateFactory
{
    IMonsterState CreateIdleState();
    IMonsterState CreateSuspiciousState();
    IMonsterState CreateSearchState();
    IMonsterState CreateAlertState();
    IMonsterState CreateChaseState();
    IMonsterState CreateAttackState();
    IMonsterState CreateStaggerState(float stunTime);


    IMonsterState GetStateForPerception(MonsterPerceptionState state);
}
