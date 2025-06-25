using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : IMonsterState
{
    private BaseMonster monster;
    private float attackCooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        attackCooldown = monster.data.attackCooldown;
        timer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Combat); // ���� ���� ����
        monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();

        Debug.Log($"[{monster.name}] ����: Attack ����");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        if (!monster.IsInAttackRange())
        {
            var chaseState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(chaseState);
            return;
        }

        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;
            //monster.PerformAttack(); // ���� ������ ���ο��� ó��
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ����: Attack ����");
    }

    // �ִϸ��̼� �̺�Ʈ�� ������ ���� �Լ�
    private void ApplyDamage()
    {
        var target = monster.GetTarget();
        if (target != null)
        {
            Debug.Log($"[{monster.name}] �� {target.name} ���� {monster.data.attackPower} ������");

            // ���� ������ ������ �������̽� ��� ���� ��õ
            // ex: target.GetComponent<IDamageable>()?.TakeDamage(monster.data.attackPower);
        }
    }
}

