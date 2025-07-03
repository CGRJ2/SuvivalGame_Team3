using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossPhase2AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private BossMonsterDataSO bossData;
    private List<BossAttackPattern> phase2Patterns;
    private float attackCooldown;
    private float timer;
    private float phase2AttackACooldownTimer;

    private enum AttackPhase { None, Prelude, Attack, AfterDelay }
    private AttackPhase attackPhase = AttackPhase.None;
    public BossAttackPattern CurrentPattern { get; private set; }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        bossData = monster.data as BossMonsterDataSO;
        phase2Patterns = bossData.phase2PatternSO?.patterns;

        attackPhase = AttackPhase.Prelude;
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
        // �������� ������ ������ Idle����
        if (monster.IsOutsideActionRadius())
        {
            monster.StateMachine.ChangeState(new MonsterIdleState());
            return;
        }
        // ���� ��Ÿ��
        timer += Time.deltaTime;
        phase2AttackACooldownTimer -= Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;

            if (phase2AttackACooldownTimer <= 0f && Random.value < 0.4f)
            {
                // 1. SO���� ���� ���� ����
                if (phase2Patterns != null && phase2Patterns.Count > 0)
                    CurrentPattern = phase2Patterns[Random.Range(0, phase2Patterns.Count)];
                else
                    CurrentPattern = null; // fallback

                monster.GetComponent<MonsterView>()?.PlayMonsterPhase2PreludeAnimation();
                phase2AttackACooldownTimer = bossData.Phase2AttackCooldown;
                Debug.Log($"[{monster.name}] Phase2 Ư������ �ߵ�! (��ٿ� ����)");
            }
            else
            {
                monster.TryAttack();
            }

        }
    }
    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack ����");
    }
}

