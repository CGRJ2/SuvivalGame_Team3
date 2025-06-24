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
        Debug.Log($"[{monster.name}] CatIdle 상태 진입");
    }

    public void Execute()
    {
        // 감지 우선 (즉시 반응)
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

        // alertLevel이 일정 수준 이상이면 탐색으로 전이
        if (monster.AlertLevel >= 10f)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Search);
            // 추후 SearchState 구현되면 전이
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] CatIdle 상태 종료");
    }
}
