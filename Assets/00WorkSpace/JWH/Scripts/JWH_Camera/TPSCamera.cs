using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TpsCamera : ICamera
{
    
    public Transform playerTarget;           // 플레이어
    public Transform cameraTransform;  // 카메라
    public Vector3 offset = new Vector3(0, 3, -5); // 시야 위치
    private float rotationSpeed = 5f;

    private float yaw;
    private float pitch;
    private float currentZoom;

    private float zoomSpeed = 2f;
    private float minZoom = 2f;
    private float maxZoom = 10f;


    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }

    public void InitializeCam(Transform tpsplayerTarget, Transform tpscameraTransform)
    {
        playerTarget = tpsplayerTarget;
        cameraTransform = tpscameraTransform;
        currentZoom = offset.magnitude; 
        
        yaw = tpsplayerTarget.eulerAngles.y;
        pitch = 0f;
    }

    public void ActivateCam(Transform previousActiveCameraTransform)
    {
        if (cameraTransform != null)
        {
            Debug.Log("3인칭 카메라 활성화");
        }
    }

    public void DeactivateCam()
    {
        if (cameraTransform != null)
        {
            Debug.Log("3인칭 카메라 비활성화");
        }
    }

   

    public void UpdateCam(Transform playerTarget)
    {
        if (playerTarget == null || cameraTransform == null) return;

       
        // 회전
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -40f, 85f);

        // 줌
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // 카메라 계산
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 zoomedOffset = rotation * new Vector3(0, offset.y, -currentZoom);
        Vector3 desiredPosition = playerTarget.position + zoomedOffset;

        cameraTransform.position = desiredPosition;
        cameraTransform.LookAt(playerTarget.position);
    }
}
