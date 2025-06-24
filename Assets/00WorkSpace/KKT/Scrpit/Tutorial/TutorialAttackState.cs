using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttackState : TutorialBaseState
{
    public TutorialAttackState(TutorialManager manager) : base(manager) { }
    public override void Enter() 
    { 
        manager.ShowAttackGuideUI(true); 
    }
    public override void Update()
    {
        if (manager.PlayerDidAttack())
            manager.stateMachine.ChangeState(TutorialStateType.Attack);
    }
    public override void Exit() 
    { 
        manager.ShowAttackGuideUI(false); 
    }
}
