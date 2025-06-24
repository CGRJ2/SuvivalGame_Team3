using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSaveState : TutorialBaseState
{
    public TutorialSaveState(TutorialManager manager) : base(manager) { }
    public override void Enter()
    {
        manager.ShowSaveGuideUI(true);
    }
    public override void Update()
    {
        if (manager.PlayerDidSave())
            manager.stateMachine.ChangeState(TutorialStateType.Save);
    }
    public override void Exit()
    {
        manager.ShowSaveGuideUI(false);
    }
}
