using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdleState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
        Debug.Log($"[{monster.name}] CatIdle ���� ����");
    }

    public void Execute()
    {
        bool canSeePlayer = monster.IsInSight();

        // ���� �켱 (��� ����)
        if (monster.IsDead)
        {
            monster.StateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        if (canSeePlayer)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(new MonsterChaseState());
            return;
        }

        if (monster.AlertLevel >= monster.AlertThreshold_Medium)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Search);
            monster.StateMachine.ChangeState(new CatSearchState());
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] CatIdle ���� ����");
    }
}
