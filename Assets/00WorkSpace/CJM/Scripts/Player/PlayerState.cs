using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    protected PlayerController pc;

    public PlayerState(PlayerController pc)
    {
        this.pc = pc;
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

public class Player_Idle : PlayerState
{
    public Player_Idle(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("idleEnter");
        pc.View.animator.SetBool("IsMove", false);
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

public class Player_Move : PlayerState
{
    public Player_Move(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("moveEnter");
    }
    
    public override void Exit()
    {
        base.Exit();
        Debug.Log("moveExit");
        
    }
    public override void Update()
    {
        base.Update();
    }
}

public class Player_Sprint : PlayerState
{
    public Player_Sprint(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        pc.View.animator.SetBool("IsSprint", true);
    }

    public override void Exit()
    {
        pc.View.animator.SetBool("IsSprint", false);
    }

    public override void Update()
    {
        // 배터리 더 빨리 감소
    }
}

public class Player_Jump : PlayerState
{
    public Player_Jump(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        pc.View.animator.SetTrigger("Jump");
        pc.View.Jump(pc.Status.JumpForce);
    }

    public override void Exit()
    {
        base.Exit();

    }
}

public class Player_Fall : PlayerState
{
    public Player_Fall(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("EnterFall");
        pc.View.animator.SetBool("IsFalling", true);
    }
    
    public override void Exit()
    {
        base.Exit();
        Debug.Log("ExitFall");
        pc.View.animator.SetBool("IsFalling", false);
    }
    public override void Update()
    {
        base.Update();
    }
}

public class Player_Crouch : PlayerState
{
    public Player_Crouch(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        pc.View.animator.SetBool("IsCrouch", true);
    }

    public override void Exit()
    {
        base.Exit();
        pc.View.animator.SetBool("IsCrouch", false);
    }
    public override void Update()
    {
        base.Update();
    }
}

public class Player_Attack : PlayerState
{
    public Player_Attack(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("EnterAttack");
        pc.View.animator.SetTrigger("Attack");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("ExitAttack");
    }
    public override void Update()
    {
        base.Update();
    }
}


