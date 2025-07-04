using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : IMonsterState
{
    private BaseMonster monster;
    private bool animationPlayed = false;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        animationPlayed = false;
        monster.Agent.enabled = false;
        if (monster.IsDead)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterDeathAnimation();
            animationPlayed = true;

            Debug.Log($"[{monster.name}] ����: Dead ����");
        }
        else
        {
            Debug.LogWarning($"[{monster.name}] Dead ���� ���� ��û �� �׷��� isDead == false");
        }
    }

    public void Execute()
    {
        // ��� ���¿����� �ƹ��͵� ���� ����
        // ���� �ִϸ��̼� �Ϸ� �� �ı� �� Ÿ�̹� ��� ó�� �ʿ� �� ���⿡ �ۼ�
    }

    public void Exit()
    {
        // ���� ���´� ������� �����Ƿ� Ư���� ó�� ����
        Debug.Log($"[{monster.name}] Dead ���¿��� Exit ȣ�� (������ ��Ȳ�� �� ����)");
    }
}