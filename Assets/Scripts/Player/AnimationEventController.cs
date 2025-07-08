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

    // �ִϸ��̼� �̺�Ʈ
    public void Hit()
    {
        pc.Attack();
    }

    // �ִϸ��̼� �̺�Ʈ
    public void AttackMotionOver()
    {
        pc.View.animator.SetBool("IsAttack", false);
        pc.stateMachine.ChangeState(pc.stateMachine.stateDic[PlayerStateTypes.Idle]);
    }


    // ������ ���� �� => ��� ���� �ʱ�ȭ
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
