using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        monster.GetComponent<MonsterView>()?.PlayMonsterDeathAnimation();
        Debug.Log($"[{monster.name}] 상태: Dead 진입");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        // 죽은 상태는 종료되지 않음
    }
}
