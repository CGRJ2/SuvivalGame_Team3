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

        // 미끼와 플레이어 분기
        Vector3 toTarget = target.position - owner.transform.position;
        toTarget.y = 0;

        if (targetType == OwnerAI.OwnerDetectionTarget.OwnerBait)
        {
            owner.Move(toTarget.normalized * 1f); // 미끼 속도조절 가능
            if (toTarget.magnitude < 1.2f)
            {
                owner.ApplyPacifyEffect(6f);  
                Debug.Log($"[{owner.name}] 미끼 도달 => 무력화 상태 전이");
            }
        }
        else if (targetType == OwnerAI.OwnerDetectionTarget.Player)
        {
            owner.Move(toTarget.normalized * 1.0f);
            // 공격 사거리 도달 시 잡기 등
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