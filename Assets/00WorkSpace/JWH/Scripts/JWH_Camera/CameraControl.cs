using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private ICamera currentStrategy;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void SetCamera(ICamera cameraStrategy)// 카메라 전략 변경
    {
        currentStrategy = cameraStrategy;
    }

    public void SetTarget(Transform newTarget) //타겟 설정
    {
        target = newTarget;
    }

    public void SetOffset(Vector3 newOffset) // 오프셋 설정
    {
        offset = newOffset;
    }

    public Transform GetTarget()// 타겟 외부 읽기
    {
        return target;
    }

    public Vector3 GetOffset()// 오프셋 외부 읽기
    {
        return offset;
    }

    private void LateUpdate()// 프레임마다 프레임 갱신
    {
        if (currentStrategy != null)
        {
            currentStrategy.UpdateCamera(this);
        }
    }

}
