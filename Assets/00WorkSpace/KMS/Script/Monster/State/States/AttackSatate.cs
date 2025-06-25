using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IMonsterState
{
    private BaseMonster monster;
    private float attackCooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        attackCooldown = monster.data.attackCooldown;
        timer = 0f;

        monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();
        Debug.Log($"[{monster.name}] 상태: Attack 진입");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            // 데미지 적용은 별도 메서드로 빼도 됨
            ApplyDamage();
            timer = 0f;
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Attack 종료");
    }

    private void ApplyDamage()
    {
        var target = monster.GetTarget();
        if (target != null)
        {
            Debug.Log($"[{monster.name}] → {target.name} 에게 {monster.data.attackPower} 데미지");
            // 실제 타겟에 데미지를 주는 로직은 따로 구성 필요
        }
    }
}

