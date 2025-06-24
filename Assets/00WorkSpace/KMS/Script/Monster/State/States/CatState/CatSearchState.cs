using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSearchState : IMonsterState
{
    private BaseMonster monster;
    private float searchDuration = 4f;
    private float timer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        monster.GetComponent<MonsterView>()?.PlayIdleAnimation(); // 수색 애니메이션이 있다면 이걸로 교체
        Debug.Log($"[{monster.name}] CatSearch 상태 진입");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        bool canSeePlayer = monster.IsInSight();

        if (monster.IsDead)
        {
            monster.StateMachine.ChangeState(new DeadState());
            return;
        }

        if (canSeePlayer)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(new ChaseState());
            return;
        }

        if (timer >= searchDuration)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Idle);
            monster.StateMachine.ChangeState(new CatIdleState());
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] CatSearch 상태 종료");
    }
}