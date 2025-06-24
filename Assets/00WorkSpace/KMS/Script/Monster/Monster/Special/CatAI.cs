using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : BaseMonster
{
    private bool isPacified = false; // ����ȭ
    public bool IsPacified => isPacified;
    private float pacifyTimer = 0f;  // ����ȭ �ð�

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

    public void ApplyPacifyEffect(float duration) // ������ ������ ����ȭ �ð��� ���
    {
        isPacified = true;
        pacifyTimer = duration;
        SetPerceptionState(MonsterPerceptionState.Idle);
    }
}