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
        Debug.Log($"[{monster.name}] ����: Dead ����");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        // ���� ���´� ������� ����
    }
}
