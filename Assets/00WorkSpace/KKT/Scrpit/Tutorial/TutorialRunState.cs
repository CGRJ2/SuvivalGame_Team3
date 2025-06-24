using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRunState : TutorialBaseState
{
    public TutorialRunState(TutorialManager manager) : base(manager) { }

    public override void Enter()
    {
        manager.ShowRunGuideUI(true);
    }

    public override void Update()
    {
        if (manager.PlayerDidRun())
            manager.stateMachine.ChangeState(TutorialStateType.Jump);
    }

    public override void Exit()
    {
        manager.ShowRunGuideUI(false);
    }
}
