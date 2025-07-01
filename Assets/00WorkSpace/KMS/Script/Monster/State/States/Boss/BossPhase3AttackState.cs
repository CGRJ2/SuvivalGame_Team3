using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossPhase3AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private BossMonsterDataSO bossData;
    private float attackCooldown;
    private float phase2AttackACooldown;
    private float phase3AttackACooldown;
    private float timer;
    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        attackCooldown = monster.data.AttackCooldown;

        // SO ���� & ĳ����
        BossMonsterDataSO bossData = monster.data as BossMonsterDataSO;
        if (bossData != null)
        {
            phase2AttackACooldown = bossData.Phase2AttackCooldown;
            phase3AttackACooldown = bossData.Phase3AttackCooldown;
        }
        else
        {
            Debug.LogError("BossMonsterDataSO Ÿ���� �ƴ�");
        }

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
        phase3AttackACooldown -= Time.deltaTime;

        if (phase2AttackACooldown <= 0f && Random.value < 0.3f)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase2AttackACooldown = bossData.Phase2AttackCooldown; // SO���� �ٽ� �ʱ�ȭ
        }
        else if (phase3AttackACooldown <= 0f && Random.value < 0.4f)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase3TryAttack();
            phase3AttackACooldown = bossData.Phase3AttackCooldown; // SO���� �ٽ� �ʱ�ȭ
        }
        else
        {
            // �Ϲ� ����
            if (timer >= attackCooldown)
            {
                timer = 0f;
                monster.TryAttack();
            }
        }
    }
    public void Exit()
    {
        
        Debug.Log($"[{monster.name}] Phase3Attack ����");
    }
}
