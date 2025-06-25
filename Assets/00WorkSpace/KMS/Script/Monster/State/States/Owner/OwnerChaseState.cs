using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerChaseState : IMonsterState
{
    private BaseMonster monster;
    private Transform target;
    private float lostTimer = 0f;
    private const float chaseLoseDelay = 2.5f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        target = monster.GetTarget();

        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{monster.name}] 상태: OwnerChase 진입");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // 시야에 타겟이 없고 탐지 반경 밖일 경우 탈출 카운트 증가
        if (!monster.checkTargetVisible || monster.IsOutsideDetectionRadius())
        {
            lostTimer += Time.deltaTime;

            if (lostTimer >= chaseLoseDelay)
            {
                Debug.Log($"[{monster.name}] 추적 실패 => Idle 상태 전이");
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }

            return;
        }

        lostTimer = 0f; // 추적 유지 시 초기화

        Vector3 toPlayer = target.position - monster.transform.position;
        toPlayer.y = 0;

        monster.Move(toPlayer.normalized);

        // 공격 범위 도달 시 상태 전이
       //if (monster.IsInAttackRange())
       //{
       //    Debug.Log($"[{monster.name}] 플레이어 도달 => 잡기 공격 상태 전이");
       //    monster.StateMachine.ChangeState(monster.GetAttackState());
       //}
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] OwnerChase 종료");
    }
}
