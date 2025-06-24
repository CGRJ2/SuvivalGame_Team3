using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdleState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        monster.GetComponent<MonsterView>()?.PlayIdleAnimation();
        Debug.Log($"[{monster.name}] CatIdle ���� ����");
    }

    public void Execute()
    {
        // ���� �켱 (��� ����)
        if (monster.IsDead)
        {
            monster.StateMachine.ChangeState(new DeadState());
            return;
        }

        if (monster.IsInSight())
        {
            monster.SetPerceptionState(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(new ChaseState());
            return;
        }

        // alertLevel�� ���� ���� �̻��̸� Ž������ ����
        if (monster.AlertLevel >= 10f)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Search);
            // ���� SearchState �����Ǹ� ����
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] CatIdle ���� ����");
    }
}
