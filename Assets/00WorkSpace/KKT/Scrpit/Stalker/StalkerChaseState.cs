using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerChaseState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;

    public StalkerChaseState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        stalker.anim.SetBool("IsMoving", true);
    }

    public override void Update()
    {
        if (stalker.target == null)
        {
            controller.ChangeState(StalkerStateType.Idle);
            return;
        }

        float distance = Vector3.Distance(stalker.transform.position, stalker.target.transform.position);

        // 공격 사거리 안에 들어오면 Attack 상태로
        if (distance <= stalker.status.AttackRange)
        {
            controller.ChangeState(StalkerStateType.Attack);
        }
        else
        {
            stalker.Move();
        }
    }

    public override void Exit()
    {
        stalker.anim.SetBool("IsMoving", false);
    }
}
