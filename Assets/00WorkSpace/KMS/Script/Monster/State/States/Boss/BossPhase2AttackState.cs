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

    private enum AttackPhase { None, Prelude, Attack, AfterDelay }
    private AttackPhase attackPhase = AttackPhase.None;

    private float timer = 0f;
    private float preludeTime = 1.0f;      // SO에서 읽음
    private float afterDelay = 0.7f;       // SO에서 읽음

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        bossData = monster.data as BossMonsterDataSO;

        preludeTime = currentPattern.preludeTime;   // SO에서 패턴별 값 읽기
        afterDelay = currentPattern.afterDelay;

        attackPhase = AttackPhase.Prelude;
        timer = 0f;

        monster.GetComponent<MonsterView>()?.PlayMonsterPhase2PreludeAnimation();
        // 공격 실행(phase2TryAttack)은 반드시 애니메이션 이벤트에서 호출
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        timer += Time.deltaTime;

        if (attackPhase == AttackPhase.Prelude)
        {
            if (timer >= preludeTime)
            {
                attackPhase = AttackPhase.Attack;
                timer = 0f;
                // 공격 모션/데미지 등은 애니메이션 이벤트에서 호출
            }
        }
        else if (attackPhase == AttackPhase.Attack)
        {
            // 애니메이션 이벤트가 오면 → attackPhase = AttackPhase.AfterDelay; timer=0;
            // 예시로 바로 넘긴다면:
            attackPhase = AttackPhase.AfterDelay;
            timer = 0f;
        }
        else if (attackPhase == AttackPhase.AfterDelay)
        {
            if (timer >= afterDelay)
            {
                // 후딜 끝나면 Idle로 전이(혹은 다음 상태)
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack 종료");
    }
}

