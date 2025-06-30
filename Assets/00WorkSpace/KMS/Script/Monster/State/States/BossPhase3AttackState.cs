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

        // 공격 쿨타임
        timer += Time.deltaTime;
        phase2AttackACooldown -= Time.deltaTime;
        if (phase2AttackACooldown <= 0f && Random.value < 0.3f) // 특수A
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase2AttackACooldown = 15f;
        }
        else if (phase3AttackACooldown <= 0f && Random.value < 0.4f) // 특수B
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase3AttackACooldown = 25f;
        }
        else
        {
            // 일반 공격
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
        
        Debug.Log($"[{monster.name}] Phase3Attack 종료");
    }
}
