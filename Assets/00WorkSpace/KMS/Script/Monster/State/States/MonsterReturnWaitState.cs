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
        Debug.Log($"[{monster.name}] ����: ReturnWait ���� - 3�� ��� ����");
    }

    public void Execute()
    {
        waitTimer += Time.deltaTime;
        Debug.Log($"[{monster.name}] ���� ��� ��... {waitTimer:F2}/{returnDelay} | dt:{Time.deltaTime}");

        monster.DecreaseAlert(Time.deltaTime * 50f);

        if (waitTimer >= returnDelay)
        {
            Debug.Log($"[{monster.name}] 3�� ��� - Return ���·� ����");
            monster.StateMachine.ChangeState(new MonsterReturnState());
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ReturnWait ���� ����");
    }
}