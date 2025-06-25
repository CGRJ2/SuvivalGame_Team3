using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DefaultMonsterSensor : IMonsterSensor
{
    private readonly bool debug;

    public DefaultMonsterSensor(bool debug = false)
    {
        this.debug = debug;
    }

    public bool IsTargetVisible(Transform self, Transform target, float detectionRange, float fov, float eyeHeight)
    {
        if (target == null) return false;

        Vector3 eyePosition = self.position + Vector3.up * eyeHeight;
        Vector3 directionToTarget = (target.position - eyePosition).normalized;
        float angle = Vector3.Angle(self.forward, directionToTarget);
        float distanceToTarget = Vector3.Distance(eyePosition, target.position);

        if (debug)
        {
            Debug.DrawRay(eyePosition, self.forward * 5f, Color.red);
            Debug.DrawRay(eyePosition, directionToTarget * 5f, Color.green);
            Debug.Log($"[SightCheck] angle={angle:F1}, fov={fov:F1}, dist={distanceToTarget:F1}, range={detectionRange:F1}");
        }

        if (angle > fov)
        {
            if (debug) Debug.Log("[SightCheck] 실패: 시야각 벗어남");
            return false;
        }

        if (distanceToTarget > detectionRange)
        {
            if (debug) Debug.Log("[SightCheck] 실패: 거리 초과");
            return false;
        }

        if (Physics.Raycast(eyePosition, directionToTarget, out RaycastHit hit, detectionRange))
        {
            if (debug) Debug.Log($"[SightCheck] Ray hit: {hit.transform.name}");
            if (hit.transform == target)
            {
                if (debug) Debug.Log("[SightCheck] 성공: 타겟 직접 감지");
                return true;
            }
            else
            {
                if (debug) Debug.Log("[SightCheck] 실패: 중간에 다른 오브젝트 감지됨");
            }
        }
        else
        {
            if (debug) Debug.Log("[SightCheck] 실패: Raycast가 아무것도 감지 못함");
        }

        return false;
    }
}