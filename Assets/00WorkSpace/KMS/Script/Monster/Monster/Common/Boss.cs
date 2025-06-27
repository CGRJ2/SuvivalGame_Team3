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
            stateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        // ������ ü�� 50% ������ �� ���� ��ȯ ����
        if (currentHP < data.MaxHP * 0.5f)
        {
            // ���� Phase2 �������� ��ȯ ����
            stateMachine.ChangeState(new BossPhase2AttackState()); // ����
            return;
        }

        if (SetPerceptionState(MonsterPerceptionState.Alert))
        {
            stateMachine.ChangeState(new MonsterChaseState());
        }
        else
        {
            stateMachine.ChangeState(new MonsterIdleState());
        }
    }
}
