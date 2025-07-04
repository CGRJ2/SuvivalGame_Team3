using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerSleepState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        monster.StateMachine.ChangeState(new MonsterIdleState(monster)); // 수면 중엔 모든 경계도/반응 off
        monster.GetComponent<MonsterView>()?.PlayMonsterSleepAnimation(); // 실제 애니메이터에서 sleep 트리거
        Debug.Log($"[{monster.name}] Sleep 상태 진입");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Sleep 상태 종료");
    }
}
