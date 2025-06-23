using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : BaseState
{
    protected PlayerView view;

    public PlayerMovementState(PlayerView viewer)
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
        Debug.Log("up");

    }
}

public class Movement_Idle : PlayerMovementState
{
    public Movement_Idle(PlayerView viewer) : base(viewer)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("idleEnter");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("idleExit");
    }
}

public class Movement_Move : PlayerMovementState
{
    public Movement_Move(PlayerView viewer) : base(viewer)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("moveEnter");

        view.animator.SetBool("IsMove", true);
    }
    
    public override void Exit()
    {
        base.Exit();
        Debug.Log("moveExit");
        view.animator.SetBool("IsMove", false);
    }
    public override void Update()
    {
        base.Update();
        Debug.Log(view.moveDir);
        
    }
}

public class Movement_Sprint : PlayerMovementState
{
    public Movement_Sprint(PlayerView viewer) : base(viewer)
    {

    }

    public override void Enter()
    {
        view.animator.SetBool("Sprint", true);
    }

    public override void Exit()
    {
        view.animator.SetBool("Sprint", false);
    }

    public override void Update()
    {

    }
}

public class Movement_Jump : PlayerMovementState
{
    public Movement_Jump(PlayerView viewer) : base(viewer)
    {

    }

    public override void Enter()
    {
        view.animator.SetTrigger("Jump");
    }
}

public class Movement_Fall : PlayerMovementState
{
    public Movement_Fall(PlayerView viewer) : base(viewer)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("EnterFall");
        view.animator.SetBool("IsFalling", true);
    }

    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("ExitFall");
        view.animator.SetBool("IsFalling", false);
    }
}


