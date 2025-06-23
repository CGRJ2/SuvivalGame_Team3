using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : BaseMonster
{
    protected override void HandleState()
    {
        if (currentHP <= 0)
        {
            stateMachine.ChangeState(new DeadState());
            return;
        }

        if (CanSeePlayer())
        {
            stateMachine.ChangeState(new ChaseState());
        }
        else
        {
            stateMachine.ChangeState(new IdleState());
        }
    }
}