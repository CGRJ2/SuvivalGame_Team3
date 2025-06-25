using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerAI : BaseMonster
{
    protected override void Awake()
    {
        base.Awake();
        stateFactory = new OwnerStateFactory(this);
    }

    protected override void HandleState()
    {
        if (IsDead)
        {
            if (!(StateMachine.CurrentState is OwnerDeadState))
                StateMachine.ChangeState(new OwnerDeadState());
            return;
        }

        if (IsInAttackRange())
        {
            if (!(StateMachine.CurrentState is OwnerAttackState))
                StateMachine.ChangeState(stateFactory.CreateAttackState());
        }
        else
        {
            if (!(StateMachine.CurrentState is OwnerChaseState))
                StateMachine.ChangeState(stateFactory.CreateChaseState());
        }
    }

    public void ThrowPlayer(Vector3 direction, float force)
    {
        var player = GetTarget()?.GetComponent<IThrowable>();
        if (player != null)
        {
            player.ApplyThrow(direction, force);
        }
    }
}