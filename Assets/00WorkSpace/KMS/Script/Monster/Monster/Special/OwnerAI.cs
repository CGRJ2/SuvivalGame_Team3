using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerAI : BaseMonster
{
    protected override void Awake()
    {
        stateFactory = new OwnerStateFactory(this);
        base.Awake();
    }

    protected override void HandleState()
    {
        if (CheckTargetVisible())
        {
            var alertState = StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            if (stateMachine.CurrentState != alertState)
                stateMachine.ChangeState(alertState);
        }
        else
        {
            var idleState = StateFactory.GetStateForPerception(MonsterPerceptionState.Idle);
            if (stateMachine.CurrentState != idleState)
                stateMachine.ChangeState(idleState);
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
    protected override void Phase2TryAttack()
    {
        throw new System.NotImplementedException();
    }
    protected override void Phase3TryAttack()
    {
        throw new System.NotImplementedException();
    }
}