using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterAI : BaseMonster
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        InitTargetByType();
        //Debug.Log($"{name}: BaseMonster.Start() ¡¯¿‘, target: {(target == null ? "null" : target.name)}");
    }
    protected override void HandleState()
    {
        if (IsDead) return;

        //Debug.Log($"[AI] target: {(target == null ? "null" : target.name)} checkTargetVisible: {checkTargetVisible}");

        if (checkTargetVisible)
        {
            SetPerceptionState(MonsterPerceptionState.Alert);
        }
    }
}