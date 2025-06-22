using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SideCamera : ICamera
{
    private Transform cameraTransform; // ����ī���� ��ġ
    private Transform playerTarget;    // �÷��̾��� ��ġ

    public Vector3 sideViewOffset = new Vector3(-5f, 2f, 0f); 
    public float transitionSpeed = 5f; // ī�޶� ��ȯ �� �ε巯�� �̵� �ӵ�

    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }

    public void InitializeCam(Transform sideplayerTarget, Transform sidecameraTransform)
    {
        playerTarget = sideplayerTarget;
        cameraTransform = sidecameraTransform;
    }

    public void ActivateCam(Transform previousActiveCameraTransform)
    {
        if (cameraTransform != null)
        {
            Debug.Log("���̵�� ī�޶� Ȱ��ȭ");
        }
    }

    public void DeactivateCam()
    {
        if (cameraTransform != null)
        {
            Debug.Log("���̵�� ī�޶� ��Ȱ��ȭ");
        }
    }

    public void UpdateCam(Transform playerTarget)
    {
        if (playerTarget == null || cameraTransform == null) return;

        
        Vector3 desiredPosition = playerTarget.position + playerTarget.TransformDirection(sideViewOffset);

       
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * transitionSpeed);

        
        cameraTransform.LookAt(playerTarget.position);
    }
}