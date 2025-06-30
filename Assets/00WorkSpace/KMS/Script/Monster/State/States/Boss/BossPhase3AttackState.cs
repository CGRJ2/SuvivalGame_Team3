using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossPhase3AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private float attackCooldown;
    private float phase2AttackACooldown;
    private float phase3AttackACooldown;
    private float timer;
    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        attackCooldown = monster.data.AttackCooldown;
        phase2AttackACooldown = bossMonster.data.Phase2AttackCooldown;
        phase3AttackACooldown = bossMonster.data.Phase3AttackCooldown;
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
        if (phase2AttackACooldown <= 0f && Random.value < 0.3f) // 특수A
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase2AttackACooldown = bossMonster.data.Phase2AttackCooldown;
        }
        else if (phase3AttackACooldown <= 0f && Random.value < 0.4f) // 특수B
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase3AttackACooldown = bossMonster.data.Phase3AttackCooldown;
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
        
        Debug.Log($"[{monster.name}] Phase3Attack 종료");
    }
}
