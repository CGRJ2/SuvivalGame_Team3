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

        // 보스는 체력 50% 이하일 때 패턴 전환 예시
        if (currentHP < data.maxHP * 0.5f)
        {
            // 추후 Phase2 패턴으로 전환 가능
            stateMachine.ChangeState(new Phase2AttackState()); // 예시
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
