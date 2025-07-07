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

        //Debug.Log($"[{monster.name}] 상태: Attack 진입");
    }

    public void Execute()
    {
        //Debug.Log($"{monster.name}공격 상태 진입");
        // 공격 범위 체크
        monster.UpdateAttackRange();

        if (monster == null || monster.IsDead) return;

        // 플레이어가 공격 범위에 없으면 다시 움직임
        if (monster.playerInRange == null)
        {
            var chaseState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(chaseState);
            
            // 몬스터 다시 움직임 활성화
            if (monster.Agent.isOnNavMesh) monster.Agent.isStopped = false;
            monster.isAttackReady = false; // 공격 대기상태 해제
            return;
        }

        // 범위 안에 플레이어가 있다면 일시 정지 이후 선딜 시간동안 대기
        // 에이전트 정지
        monster.Agent.isStopped = true;
        monster.isAttackReady = true; // 공격 대기상태(선딜)

        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;
            monster.TryAttack();
        }
    }

    public void Exit()
    {
        //Debug.Log($"[{monster.name}] 상태: Attack 종료");
    }
}

