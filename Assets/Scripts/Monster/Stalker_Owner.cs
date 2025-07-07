using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker_Owner : BaseMonster
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        InitTargetByType();
    }
    protected override void HandleState()
    {
        if (IsDead) return;

        if (checkTargetVisible)
        {
            SetPerceptionState(MonsterPerceptionState.Alert);
        }
    }
}
