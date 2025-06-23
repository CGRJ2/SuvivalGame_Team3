using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2AttackState : IMonsterState
{
    private BaseMonster monster;
    private float attackCooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        // NOTE : �Ϲ� ���Ϳ� ����ȭ�Ǵ� ����, ���� �ֱ� ª�� ����
        attackCooldown = monster.data.attackCooldown * 0.8f;
        timer = 0f;

        monster.GetComponent<MonsterView>()?.PlayAttackAnimation();
        Debug.Log($"[{monster.name}] ����: Phase2 Attack ���� (�ӵ� ����)");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            ApplyPhase2Damage();
            timer = 0f;
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack ����");
    }

    private void ApplyPhase2Damage()
    {
        var target = monster.GetTarget();
        if (target != null)
        {
            // TODO : 2������� ����� ���� ���ɼ��� �־ ������.
            float finalDamage = monster.data.attackPower * 1.5f; 
            Debug.Log($"[{monster.name}] �� {target.name} ���� ��ȭ�� {finalDamage} ������");
            // ���� ������ ���� ���� �ʿ�
        }
    }
}

