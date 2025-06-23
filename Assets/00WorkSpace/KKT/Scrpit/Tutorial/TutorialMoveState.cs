using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMoveState : TutorialBaseState
{
    public TutorialMoveState(TutorialManager manager) : base(manager) { }

    public override void Enter()
    {
        manager.ShowMoveGuideUI(true);
    }

    public override void Update()
    {
        if (manager.PlayerDidMove())
            manager.stateMachine.ChangeState(TutorialStateType.Jump);
    }

    public override void Exit()
    {
        manager.ShowMoveGuideUI(false);
    }
}
