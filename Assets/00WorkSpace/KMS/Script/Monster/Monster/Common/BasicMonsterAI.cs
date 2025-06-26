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
    protected override void OnDrawGizmosSelected()
    {
        if (data == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * data.EyeHeight, currentDetectionRange);

        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -currentFOV / 2, 0) * forward;
        Vector3 rightLimit = Quaternion.Euler(0, currentFOV / 2, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * data.EyeHeight, transform.position + leftLimit * currentDetectionRange);
        Gizmos.DrawLine(transform.position + Vector3.up * data.EyeHeight, transform.position + rightLimit * currentDetectionRange);
    }
}
