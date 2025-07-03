using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerChaseState : IMonsterState
{
    private OwnerAI owner;
    private Transform target;
    private float lostTimer = 0f;
    private const float chaseLoseDelay = 2.5f;

    public void Enter(BaseMonster monster)
    {
        owner = monster as OwnerAI;
        target = null;
        lostTimer = 0f;
        owner.RefreshBaitList();
        owner.SetPerceptionState(MonsterPerceptionState.Alert);
        owner.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{owner.name}] 상태: OwnerChase 진입");
    }

    public void Execute()
    {
        if (owner == null || owner.IsDead) return;

        // 타겟 우선순위 탐색 (미끼 > 플레이어)
        OwnerAI.OwnerDetectionTarget targetType = owner.GetClosestTarget(out target);

        if (target == null || targetType == OwnerAI.OwnerDetectionTarget.None)
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= chaseLoseDelay)
            {
                Debug.Log($"[{owner.name}] 추적 실패 => Idle 상태 전이");
                owner.StateMachine.ChangeState(new OwnerIdleState());
            }
            return;
        }
        else
        {
            lostTimer = 0f;
        }

        Vector3 destination = target.position;
        destination.y = owner.transform.position.y; // y 보정

        owner.Agent.speed = owner.OwnerData.moveSpeed * (targetType == OwnerAI.OwnerDetectionTarget.OwnerBait ? 0.9f : 1.0f);
        owner.Agent.SetDestination(destination);

        if (targetType == OwnerAI.OwnerDetectionTarget.OwnerBait)
        {
            if (Vector3.Distance(owner.transform.position, destination) < 1.2f)
            {
                owner.ApplyPacifyEffect(4f);
                Debug.Log($"[{owner.name}] 미끼 도달 => 무력화 상태 전이");
            }
        }
        else if (targetType == OwnerAI.OwnerDetectionTarget.Player)
        {
            if (owner.IsInAttackRange())
            {
                Debug.Log($"[{owner.name}] 플레이어 도달 => 잡기 공격 상태 전이");
                owner.StateMachine.ChangeState(owner.GetAttackState());
            }
        }
    }
    public void Exit()
    {
        Debug.Log($"[{owner.name}] OwnerChase 종료");
    }
}