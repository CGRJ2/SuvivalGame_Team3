using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProductionState : TutorialBaseState
{
    public TutorialProductionState(TutorialManager manager) : base(manager) { }

    public override void Enter()
    {
        manager.ShowProductionGuideUI(true);
    }

    public override void Update()
    {
        if (manager.PlayerDidProduction())
            manager.stateMachine.ChangeState(TutorialStateType.Production);
    }

    public override void Exit()
    {
        manager.ShowProductionGuideUI(false);
    }
}
