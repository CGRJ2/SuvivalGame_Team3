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
            if (debug) Debug.Log("[SightCheck] ����: �þ߰� ���");
            return false;
        }

        if (distanceToTarget > detectionRange)
        {
            if (debug) Debug.Log("[SightCheck] ����: �Ÿ� �ʰ�");
            return false;
        }

        if (Physics.Raycast(eyePosition, directionToTarget, out RaycastHit hit, detectionRange))
        {
            if (debug) Debug.Log($"[SightCheck] Ray hit: {hit.transform.name}");
            if (hit.transform == target)
            {
                if (debug) Debug.Log("[SightCheck] ����: Ÿ�� ���� ����");
                return true;
            }
            else
            {
                if (debug) Debug.Log("[SightCheck] ����: �߰��� �ٸ� ������Ʈ ������");
            }
        }
        else
        {
            if (debug) Debug.Log("[SightCheck] ����: Raycast�� �ƹ��͵� ���� ����");
        }

        return false;
    }
}