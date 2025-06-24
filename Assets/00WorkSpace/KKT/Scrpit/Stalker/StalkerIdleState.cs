using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class StalkerIdleState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;
    private float idleTime = 0f;
    private float waitDuration = 0f; // 대기시간

    public StalkerIdleState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        stalker.anim.SetBool("IsMoving", false);
        waitDuration = Random.Range(2f, 5f); // 2~5초 랜덤
        idleTime = 0f;
    }

    public override void Update()
    {
        stalker.FindTarget();
        if (stalker.target != null)
        {
            controller.ChangeState(StalkerStateType.Chase);
            return;
        }

        idleTime += Time.deltaTime;
        if (idleTime >= waitDuration)
        {
            controller.ChangeState(StalkerStateType.Move);
        }

    }

    public override void Exit()
    {
        idleTime = 0f;
    }
}