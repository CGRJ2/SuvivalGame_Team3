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
        idleTime = Random.Range(1f, 3f); // ��� �ð� ����
        monster.GetComponent<MonsterView>()?.PlayIdleAnimation();
        Debug.Log($"[{monster.name}] ����: Idle ����");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            Debug.Log($"[{monster.name}] Idle ����, ���� ���� �ʿ�");
            // ���¸ӽſ��� ���򰡰� �Ͼ �� �ֵ��� ��
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Idle ����");
    }
}


