using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase3AttackState : IMonsterState
{
    public BossAttackPattern CurrentPattern { get; private set; }
    private BaseMonster monster;
    private BossMonster bossMonster;
    private BossMonsterDataSO bossData;
    private List<BossAttackPattern> phase2Patterns;
    private List<BossAttackPattern> phase3Patterns;
    private float attackCooldown;
    private float timer;

    private float phase2AttackACooldownTimer;
    private float phase3AttackACooldownTimer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        bossData = monster.data as BossMonsterDataSO;

        phase2Patterns = bossData.phase2PatternSO?.patterns;
        phase3Patterns = bossData.phase3PatternSO?.patterns;

        attackCooldown = bossData.AttackCooldown;
        timer = 0f;
        phase2AttackACooldownTimer = 0f;
        phase3AttackACooldownTimer = 0f;
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
        if (monster.IsOutsideDetectionRadius())
        {
            monster.StateMachine.ChangeState(new MonsterIdleState(bossMonster));
            return;
        }

        timer += Time.deltaTime;
        phase2AttackACooldownTimer -= Time.deltaTime;
        phase3AttackACooldownTimer -= Time.deltaTime;

        if (timer >= attackCooldown)
        {
            timer = 0f;

            if (phase2AttackACooldownTimer <= 0f && Random.value < 0.3f)
            {
                // 1. SO에서 패턴 랜덤 선택
                if (phase2Patterns != null && phase2Patterns.Count > 0)
                    CurrentPattern = phase2Patterns[Random.Range(0, phase2Patterns.Count)];
                else
                    CurrentPattern = null; // fallback

                monster.GetComponent<MonsterView>()?.PlayMonsterPhase2PreludeAnimation();
                phase2AttackACooldownTimer = bossData.Phase2AttackCooldown;
                Debug.Log($"[{monster.name}] Phase2 특수공격 발동! (쿨다운 적용)");
            }
            else if (phase3AttackACooldownTimer <= 0f && Random.value < 0.4f)
            {
                // 1. SO에서 패턴 랜덤 선택
                if (phase3Patterns != null && phase3Patterns.Count > 0)
                    CurrentPattern = phase3Patterns[Random.Range(0, phase3Patterns.Count)];
                else
                    CurrentPattern = null; // fallback

                monster.GetComponent<MonsterView>()?.PlayMonsterPhase3PreludeAnimation();
                phase3AttackACooldownTimer = bossData.Phase3AttackCooldown;
                Debug.Log($"[{monster.name}] Phase2 특수공격 발동! (쿨다운 적용)");
            }
            else
            {
                monster.TryAttack();
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase3Attack 종료");
    }
}
