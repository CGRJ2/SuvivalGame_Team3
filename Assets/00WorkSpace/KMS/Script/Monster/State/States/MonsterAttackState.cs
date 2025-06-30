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
        attackCooldown = monster.data.AttackCooldown;
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
            monster.TryAttack();
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: Attack 종료");
    }
}

