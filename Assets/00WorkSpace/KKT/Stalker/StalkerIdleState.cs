using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class StalkerIdleState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;

    public StalkerIdleState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        stalker.anim.SetBool("IsMoving", false);
    }

    public override void Update()
    {
        stalker.FindTarget();
        if (stalker.target != null)
        {
            controller.ChangeState(StalkerStateType.Chase);
        }
    }

    public override void Exit() { }
}