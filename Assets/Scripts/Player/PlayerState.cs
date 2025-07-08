using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        pc.stateMachine.LastState = this;
    }

    public override void Update()
    {

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
        pc.View.animator.SetBool("IsMove", false);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        
        base.Exit();
    }
}

public class Player_Move : PlayerState
{
    public Player_Move(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        pc.isSprinting = false;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
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
        base.Enter();
        pc.isSprinting = true;
        pc.View.animator.SetBool("IsSprint", true);
    }

    public override void Exit()
    {
        base.Exit();
        pc.View.animator.SetBool("IsSprint", false);
    }

    public override void Update()
    {
        base.Update();
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
        pc.View.Jump(pc.Status.JumpForce);
        pc.View.animator.SetBool("IsJump", true);
    }

    public override void Exit()
    {
        pc.jumpCooling = true;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

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
        pc.jumpCooling = false;
        pc.View.animator.SetBool("IsFalling", true);
    }
    
    public override void Exit()
    {
        base.Exit();
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
        pc.Cc.SetColliderCrouch();
    }

    public override void Exit()
    {
        base.Exit();
        pc.View.animator.SetBool("IsCrouch", false);
        pc.Cc.SetColliderDefault();
        pc.CrouchToggleChange(false);
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
        //Debug.LogError("공격 상태 진입");
        pc.View.animator.SetBool("IsAttack", true);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.LogError("공격 상태 나감");
        pc.View.animator.SetBool("IsAttack", false);
    }
    public override void Update()
    {
        base.Update();
    }


}

// 1순위 상태
public class Player_Damaged : PlayerState
{
    public Player_Damaged(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        pc.Status.isControllLocked = true;
        pc.View.animator.SetTrigger("IsDamagedTrigger");
        pc.View.animator.SetBool("IsDamaged", true);
        pc.View.animator.SetBool("IsAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
        pc.View.animator.SetBool("IsDamaged", false);
    }

    public override void Update()
    {
        base.Update();
    }
}

public class Player_Dead : PlayerState
{
    public Player_Dead(PlayerController pc) : base(pc)
    {

    }

    public override void Enter()
    {
        base.Enter();
        pc.Status.isControllLocked = true;
        //Debug.LogError("EnterDead");

        //pc.View.animator.SetBool("IsDead", true);
        pc.View.animator.SetTrigger("IsDeadTrigger");
    }

    public override void Exit()
    {
        base.Exit();
        pc.View.animator.SetTrigger("Respawn");
        
        //pc.View.animator.SetBool("IsDead", false);
    }
    public override void Update()
    {
        base.Update();
    }
}



