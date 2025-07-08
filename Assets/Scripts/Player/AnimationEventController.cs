using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    PlayerController pc;
    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }


    public void JumpTop()
    {
        pc.View.animator.SetBool("IsJump", false);
        pc.stateMachine.ChangeState(pc.stateMachine.stateDic[PlayerStateTypes.Fall]);
        
    }

    // 애니메이션 이벤트
    public void Hit()
    {
        pc.Attack();
    }

    // 애니메이션 이벤트
    public void AttackMotionOver()
    {
        pc.View.animator.SetBool("IsAttack", false);
        pc.stateMachine.ChangeState(pc.stateMachine.stateDic[PlayerStateTypes.Idle]);
    }


    // 데미지 받을 시 => 모든 상태 초기화
    public void DamagedMotionEnd()
    {
        pc.View.animator.SetBool("IsAttack", false);
        pc.View.animator.SetBool("IsDamaged", false);
        pc.View.animator.SetBool("IsJump", false);
        pc.View.animator.SetBool("IsFalling", false);
        pc.Status.isControllLocked = false;

        pc.stateMachine.ChangeState(pc.stateMachine.stateDic[PlayerStateTypes.Idle]);
    }

    public void RespawnMotionEnd()
    {
        pc.Status.isControllLocked = false;
    }
}
