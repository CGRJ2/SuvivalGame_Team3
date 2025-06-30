using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossPhase3AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private float attackCooldown;
    private float phase2AttackACooldown = 0f;
    private float phase3AttackACooldown = 0f;
    private float timer;
    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        attackCooldown = monster.data.AttackCooldown;
        timer = 0f;
        phase2AttackACooldown = 0f;
        phase3AttackACooldown = 0f;
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

        // ���� ��Ÿ��
        timer += Time.deltaTime;
        phase2AttackACooldown -= Time.deltaTime;
        if (phase2AttackACooldown <= 0f && Random.value < 0.3f) // Ư��A
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase2AttackACooldown = 15f;
        }
        else if (phase3AttackACooldown <= 0f && Random.value < 0.4f) // Ư��B
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase3AttackACooldown = 25f;
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
        
        Debug.Log($"[{monster.name}] Phase3Attack ����");
    }
}
