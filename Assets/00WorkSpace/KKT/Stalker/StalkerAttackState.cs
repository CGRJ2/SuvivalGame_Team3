using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerAttackState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;
    private bool isAttacking = false;

    public StalkerAttackState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        isAttacking = false;
        stalker.rigid.velocity = Vector3.zero; // 움직임 멈춤
        stalker.anim.SetBool("IsMoving", false);
        stalker.Attack(); // 애니메이션 트리거, 데미지는 애니메이션 이벤트에서
        isAttacking = true;
    }

    public override void Update()
    {
        if (stalker.target == null)
        {
            controller.ChangeState(StalkerStateType.Idle);
            return;
        }

        float distance = Vector3.Distance(stalker.transform.position, stalker.target.transform.position);

        // 사거리 밖으로 벗어나면 추적 상태로
        if (distance > stalker.status.AttackRange)
        {
            controller.ChangeState(StalkerStateType.Chase);
            return;
        }

        // 공격 쿨타임/애니메이션 상태 등에 따라 재공격 로직 추가 가능
        // 예시: 공격 쿨타임 후 재공격, 또는 애니메이션 이벤트에서 ChangeState 호출
    }

    public override void Exit() { }
}
