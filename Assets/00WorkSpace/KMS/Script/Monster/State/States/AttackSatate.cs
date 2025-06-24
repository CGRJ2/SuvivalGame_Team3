using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IMonsterState
{
    private BaseMonster monster;
    private float attackCooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        attackCooldown = monster.data.attackCooldown;
        timer = 0f;

        monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();
        Debug.Log($"[{monster.name}] ����: Attack ����");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            // ������ ������ ���� �޼���� ���� ��
            ApplyDamage();
            timer = 0f;
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Attack ����");
    }

    private void ApplyDamage()
    {
        var target = monster.GetTarget();
        if (target != null)
        {
            Debug.Log($"[{monster.name}] �� {target.name} ���� {monster.data.attackPower} ������");
            // ���� Ÿ�ٿ� �������� �ִ� ������ ���� ���� �ʿ�
        }
    }
}

