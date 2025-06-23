using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    protected PlayerView view;
    
    public PlayerState(PlayerView viewer)
    {
        this.view = viewer;
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class Player_Idle : PlayerState
{
    public Player_Idle(PlayerView viewer) : base(viewer)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

    }

}


