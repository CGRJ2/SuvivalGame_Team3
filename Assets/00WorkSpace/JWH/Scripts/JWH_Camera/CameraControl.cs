using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private ICamera currentStrategy;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void SetCamera(ICamera cameraStrategy)// ī�޶� ���� ����
    {
        currentStrategy = cameraStrategy;
    }

    public void SetTarget(Transform newTarget) //Ÿ�� ����
    {
        target = newTarget;
    }

    public void SetOffset(Vector3 newOffset) // ������ ����
    {
        offset = newOffset;
    }

    public Transform GetTarget()// Ÿ�� �ܺ� �б�
    {
        return target;
    }

    public Vector3 GetOffset()// ������ �ܺ� �б�
    {
        return offset;
    }

    private void LateUpdate()// �����Ӹ��� ������ ����
    {
        if (currentStrategy != null)
        {
            currentStrategy.UpdateCamera(this);
        }
    }

}
