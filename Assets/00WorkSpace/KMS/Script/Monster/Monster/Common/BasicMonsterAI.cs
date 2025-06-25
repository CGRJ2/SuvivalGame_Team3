using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterAI : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        InitTargetByType();
    }
    protected override void HandleState()
    {
        if (IsDead) return;

        Debug.Log($"[AI] checkTargetVisible: {checkTargetVisible}");
        Debug.Log($"Target: {(target == null ? "null" : target.name)}");
        Debug.Log($"FOV: {currentFOV}, Range: {currentDetectionRange}");

        if (checkTargetVisible)
        {
            SetPerceptionState(MonsterPerceptionState.Alert);
        }
    }
}
