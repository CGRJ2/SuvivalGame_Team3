using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SideCamera : ICamera
{
    private Transform cameraTransform; // 메인카레라 위치
    private Transform playerTarget;    // 플레이어의 위치

    public Vector3 sideViewOffset = new Vector3(-5f, 2f, 0f); 
    public float transitionSpeed = 5f; // 카메라 전환 시 부드러운 이동 속도

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
            Debug.Log("사이드뷰 카메라 활성화");
        }
    }

    public void DeactivateCam()
    {
        if (cameraTransform != null)
        {
            Debug.Log("사이드뷰 카메라 비활성화");
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