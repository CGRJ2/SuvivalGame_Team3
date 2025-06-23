using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Boss : BaseMonster
{
    protected override void HandleState()
    {
        if (currentHP <= 0)
        {
            stateMachine.ChangeState(new DeadState());
            return;
        }

        // ������ ü�� 50% ������ �� ���� ��ȯ ����
        if (currentHP < data.maxHP * 0.5f)
        {
            // ���� Phase2 �������� ��ȯ ����
            stateMachine.ChangeState(new Phase2AttackState()); // ����
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
