using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker_Owner : BasicMonsterAI
{
    public IMonsterState sleepState;
    public IMonsterState returnToBed;
    public IMonsterState returnToOrigin;

    [Header("잠자러 가는 위치")]
    public Transform bedTransform;


    public override void Init()
    {
        base.Init();
        sleepState = new OwnerState_Sleep(this);
        returnToBed = new OwnerState_ReturnToBed(this);
        returnToOrigin = new OwnerState_ReturnToOrigin(this);
    }
}
