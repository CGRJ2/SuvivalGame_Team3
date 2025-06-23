using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialJumpState : TutorialBaseState
{
    public TutorialJumpState(TutorialManager manager) : base(manager) { }
    public override void Enter() 
    { 
        manager.ShowJumpGuideUI(true); 
    }

    public override void Update()
    {
        if (manager.PlayerDidJump())
            manager.stateMachine.ChangeState(TutorialStateType.Attack);
    }

    public override void Exit() 
    { 
        manager.ShowJumpGuideUI(false); 
    }
}
