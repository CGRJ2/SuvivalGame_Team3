using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IMonsterState
{
    private BaseMonster monster;
    private float idleTime;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        idleTime = Random.Range(1f, 3f); // 대기 시간 랜덤
        monster.GetComponent<MonsterView>()?.PlayIdleAnimation();
        Debug.Log($"[{monster.name}] 상태: Idle 진입");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            Debug.Log($"[{monster.name}] Idle 종료, 상태 재평가 필요");
            // 상태머신에서 재평가가 일어날 수 있도록 함
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Idle 종료");
    }
}


