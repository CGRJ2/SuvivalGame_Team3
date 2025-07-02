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

    public void Execute()     // 쿨타임 감소, 공격 가능 여부 체크
    {
        if (monster == null || monster.IsDead) return;

        // 공격 사거리 체크
        if (!monster.IsInAttackRange())
        {
            // 추적 상태(또는 Alert 상태)로 전환
            var chaseState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(chaseState);
            return;
        }
        // 감지범위 밖으로 나가면 Idle상태
        if (monster.IsOutsideActionRadius())
        {
            monster.StateMachine.ChangeState(new MonsterIdleState());
            return;
        }
        // 공격 쿨타임
        timer += Time.deltaTime;
        phase2AttackACooldownTimer -= Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;

            if (phase2AttackACooldownTimer <= 0f && Random.value < 0.4f)
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
            else
            {
                monster.TryAttack();
            }

        }
    }
    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack 종료");
    }
}

