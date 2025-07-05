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
        pc.isAttacking = false;
    }

    public void DamagedMotionEnd()
    {
        pc.View.animator.SetBool("IsDamaged", false);
    }

    public void RespawnMotionEnd()
    {
        pc.Status.isControllLocked = false;
    }
}
