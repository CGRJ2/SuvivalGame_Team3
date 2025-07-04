using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : BaseMonster
{
    protected override void HandleState()
    {
        if (currentHP <= 0)
        {
            stateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        if (SetPerceptionState(MonsterPerceptionState.Alert))
        {
            stateMachine.ChangeState(new MonsterChaseState());
        }
        else
        {
            stateMachine.ChangeState(new MonsterIdleState(this));
        }
    }
    protected override void Phase2TryAttack()
    {
        throw new System.NotImplementedException();
    }
    protected override void Phase3TryAttack()
    {
        throw new System.NotImplementedException();
    }
}