using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerMoveState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;
    private Vector3 destination;

    public StalkerMoveState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        // 이동 애니메이션
        stalker.anim.SetBool("IsMoving", true);

        // 랜덤 목적지 설정 (예시: 반경 5 내 랜덤)
        destination = stalker.transform.position + Random.insideUnitSphere * 5f;
        destination.y = stalker.transform.position.y; // 지상만 이동
    }

    public override void Update()
    {
        // 플레이어 감지시 즉시 추적
        stalker.FindTarget();
        if (stalker.target != null)
        {
            controller.ChangeState(StalkerStateType.Chase);
            return;
        }

        // 목적지로 이동
        Vector3 dir = (destination - stalker.transform.position).normalized;
        stalker.transform.position += dir * stalker.status.MoveSpeed * Time.deltaTime;

        // 목적지 도달시 Idle로
        if (Vector3.Distance(stalker.transform.position, destination) < 0.2f)
        {
            controller.ChangeState(StalkerStateType.Idle);
        }
    }

    public override void Exit()
    {
        stalker.anim.SetBool("IsMoving", false);
    }
}
