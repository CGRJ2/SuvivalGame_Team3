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

        Vector3 eyePos = transform.position + Vector3.up * data.EyeHeight;

        float range = currentDetectionRange > 0.1f ? currentDetectionRange : 5f;
        float fov = currentFOV > 0.1f ? currentFOV : 90f;

        Debug.Log($"[Gizmo] EyePos: {eyePos}, Range: {range}");

        // 시야 감지 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePos, range);

        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -fov / 2, 0) * forward;
        Vector3 rightLimit = Quaternion.Euler(0, fov / 2, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePos, eyePos + leftLimit * range);
        Gizmos.DrawLine(eyePos, eyePos + rightLimit * range);

        // 행동 반경 시각화
        if (OriginPosition != Vector3.zero)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(OriginPosition, data.ActionRadius);
        }
    }
}