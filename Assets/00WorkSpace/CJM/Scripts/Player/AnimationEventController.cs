using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    PlayerStatus ps;
    private void Start()
    {
        ps = GetComponentInParent<PlayerStatus>();
    }

    public void JumpTop()
    {
        ps.movementState.ChangeState(ps.movementState.stateDic[PlayerMovementStateTypes.Fall]);
    }
}
