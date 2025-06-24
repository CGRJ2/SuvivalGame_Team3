using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : BaseMonster
{
    private bool isPacified = false; // 무력화
    public bool IsPacified => isPacified;
    private float pacifyTimer = 0f;  // 무력화 시간

    protected override void HandleState()
    {
        if (isPacified)
        {
            pacifyTimer -= Time.deltaTime;
            if (pacifyTimer <= 0f)
                isPacified = false;

            if (!(stateMachine.CurrentState is CatIdleState))
                stateMachine.ChangeState(new CatIdleState());
            return;
        }

        if (IsDead)
        {
            if (!(stateMachine.CurrentState is DeadState))
                stateMachine.ChangeState(new DeadState());
            return;
        }

        if (IsInSight())
        {
            SetPerceptionState(MonsterPerceptionState.Alert);
            if (!(stateMachine.CurrentState is ChaseState))
                stateMachine.ChangeState(new ChaseState());
        }
        else
        {
            SetPerceptionState(MonsterPerceptionState.Idle);
            if (!(stateMachine.CurrentState is IdleState))
                stateMachine.ChangeState(new IdleState());
        }
    }

    public void ApplyPacifyEffect(float duration) // 아이템 사용시의 무력화 시간을 담당
    {
        isPacified = true;
        pacifyTimer = duration;
        SetPerceptionState(MonsterPerceptionState.Idle);
    }
}