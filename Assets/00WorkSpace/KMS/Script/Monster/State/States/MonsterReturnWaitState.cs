using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnWaitState : IMonsterState
{
    private BaseMonster monster;
    private float waitTimer = 0f;
    private const float returnDelay = 3f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        waitTimer = 0f;
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
        Debug.Log($"[{monster.name}] 상태: ReturnWait 진입 - 3초 대기 시작");
    }

    public void Execute()
    {
        waitTimer += Time.deltaTime;
        Debug.Log($"[{monster.name}] 복귀 대기 중... {waitTimer:F2}/{returnDelay} | dt:{Time.deltaTime}");

        monster.DecreaseAlert(Time.deltaTime * 50f);

        if (waitTimer >= returnDelay)
        {
            Debug.Log($"[{monster.name}] 3초 경과 - Return 상태로 전이");
            monster.StateMachine.ChangeState(new MonsterReturnState());
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ReturnWait 상태 종료");
    }
}