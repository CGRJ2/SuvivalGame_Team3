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
        pc.Status.stateMachine.ChangeState(pc.Status.stateMachine.stateDic[PlayerStateTypes.Fall]);
    }

    // �ִϸ��̼� �̺�Ʈ
    public void AttackHitTime()
    {
        pc.Attack();
    }

    // �ִϸ��̼� �̺�Ʈ
    public void AttackMotionOver()
    {
        //Debug.Log("�̺�Ʈ");
    }
}
