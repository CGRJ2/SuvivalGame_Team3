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

    // 애니메이션 이벤트
    public void AttackHitTime()
    {
        pc.Attack();
    }

    // 애니메이션 이벤트
    public void AttackMotionOver()
    {
        //Debug.Log("이벤트");
    }
}
