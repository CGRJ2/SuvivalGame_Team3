using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : IMonsterState
{
    private BaseMonster monster;
    private float attackCooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        attackCooldown = monster.data.attackCooldown;
        timer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Combat); // 전투 상태 설정
        monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();

        Debug.Log($"[{monster.name}] 상태: Attack 진입");
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

        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;
            //monster.PerformAttack(); // 실제 공격은 내부에서 처리
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: Attack 종료");
    }

    // 애니메이션 이벤트용 데미지 적용 함수
    private void ApplyDamage()
    {
        var target = monster.GetTarget();
        if (target != null)
        {
            Debug.Log($"[{monster.name}] → {target.name} 에게 {monster.data.attackPower} 데미지");

            // 실제 데미지 적용은 인터페이스 기반 설계 추천
            // ex: target.GetComponent<IDamageable>()?.TakeDamage(monster.data.attackPower);
        }
    }
}

