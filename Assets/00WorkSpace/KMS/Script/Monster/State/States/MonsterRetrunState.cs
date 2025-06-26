using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;

        monster.SetPerceptionState(MonsterPerceptionState.Idle); // 돌아가는 동안 감지 안 하게 하려면
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{monster.name}] 상태: Return 진입 - 스폰지점으로 복귀 시작");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        Vector3 toSpawn = monster.GetSpawnPoint() - monster.transform.position;
        toSpawn.y = 0f;

        if (toSpawn.magnitude > 0.5f)
        {
            monster.Move(toSpawn.normalized);
        }
        else
        {
            Debug.Log($"[{monster.name}] 스폰지점 도착 → Idle 상태 전환");
            monster.StateMachine.ChangeState(monster.GetIdleState());
        }
    }
    public void Exit()
    {
        Debug.Log($"[{monster.name}] Return 종료 → 상태 초기화");

        monster.ResetAlert();

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
    }
}
