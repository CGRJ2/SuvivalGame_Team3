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

        // SO 참조 & 캐스팅
        BossMonsterDataSO bossData = monster.data as BossMonsterDataSO;
        if (bossData != null)
        {
            phase2AttackACooldown = bossData.Phase2AttackCooldown;
            phase3AttackACooldown = bossData.Phase3AttackCooldown;
        }
        else
        {
            Debug.LogError("BossMonsterDataSO 타입이 아님");
        }

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
        phase3AttackACooldown -= Time.deltaTime;

        if (phase2AttackACooldown <= 0f && Random.value < 0.3f)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase2TryAttack();
            phase2AttackACooldown = bossData.Phase2AttackCooldown; // SO에서 다시 초기화
        }
        else if (phase3AttackACooldown <= 0f && Random.value < 0.4f)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterPhase2AttackAnimation();
            bossMonster.phase3TryAttack();
            phase3AttackACooldown = bossData.Phase3AttackCooldown; // SO에서 다시 초기화
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
