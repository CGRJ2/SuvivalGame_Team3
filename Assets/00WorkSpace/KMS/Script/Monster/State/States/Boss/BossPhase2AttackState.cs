using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossPhase2AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private BossMonsterDataSO bossData;
    private float attackCooldown;
    private float phase2AttackACooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;

        bossData = monster.data as BossMonsterDataSO;
        if (bossData == null)
        {
            Debug.LogError("BossMonsterDataSO 타입이 아님");
            return;
        }

        attackCooldown = bossData.AttackCooldown;
        phase2AttackACooldown = bossData.Phase2AttackCooldown;
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
        // 감지범위 밖으로 나가면 회복 + Idle상태
        if (monster.IsOutsideActionRadius())
        {
            bossMonster.ResetBoss();
            monster.StateMachine.ChangeState(new MonsterIdleState());
            return;
        }
        // 공격 쿨타임
        timer += Time.deltaTime;
        phase2AttackACooldown -= Time.deltaTime;
        if (phase2AttackACooldown <= 0f && Random.value < 0.4f)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase2AttackACooldown = bossData.Phase2AttackCooldown; 
        }
        else
        {
            // 일반 공격
            if (timer >= attackCooldown)
            {
                timer = 0f;
                monster.TryAttack();
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack 종료");
    }
}

