using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2AttackState : IMonsterState
{
    private BaseMonster monster;
    private float attackCooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        // NOTE : 일반 몬스터와 차별화되는 패턴, 공격 주기 짧게 설정
        attackCooldown = monster.data.attackCooldown * 0.8f;
        timer = 0f;

        monster.GetComponent<MonsterView>()?.PlayAttackAnimation();
        Debug.Log($"[{monster.name}] 상태: Phase2 Attack 진입 (속도 증가)");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            ApplyPhase2Damage();
            timer = 0f;
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack 종료");
    }

    private void ApplyPhase2Damage()
    {
        var target = monster.GetTarget();
        if (target != null)
        {
            // TODO : 2페이즈에선 대미지 증가 가능성이 있어서 만들어둠.
            float finalDamage = monster.data.attackPower * 1.5f; 
            Debug.Log($"[{monster.name}] → {target.name} 에게 강화된 {finalDamage} 데미지");
            // 실제 데미지 적용 로직 필요
        }
    }
}

