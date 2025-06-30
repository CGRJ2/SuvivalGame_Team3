using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossPhase2AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private float attackCooldown;
    private float phase2AttackACooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        attackCooldown = monster.data.AttackCooldown;
        phase2AttackACooldown = bossMonster.data.Phase2AttackCooldown;
        timer = 0f;
    }

    public void Execute()     // ��Ÿ�� ����, ���� ���� ���� üũ
    {
        if (monster == null || monster.IsDead) return;

        // ���� ��Ÿ� üũ
        if (!monster.IsInAttackRange())
        {
            // ���� ����(�Ǵ� Alert ����)�� ��ȯ
            var chaseState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(chaseState);
            return;
        }
        // �������� ������ ������ ȸ�� + Idle����
        if (monster.IsOutsideActionRadius())
        {
            bossMonster.ResetBoss();
            monster.StateMachine.ChangeState(new MonsterIdleState());
            return;
        }
        // ���� ��Ÿ��
        timer += Time.deltaTime;
        phase2AttackACooldown -= Time.deltaTime;
        if (phase2AttackACooldown <= 0f && Random.value < 0.4f) // 40% Ȯ�� Ư��A
        {
            // Ư��A �ִϸ��̼� �� ����
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase2AttackACooldown = bossMonster.data.Phase2AttackCooldown;
        }
        else
        {
            // �Ϲ� ����
            if (timer >= attackCooldown)
            {
                timer = 0f;
                monster.TryAttack();
                monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack ����");
    }
}

