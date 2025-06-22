using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TpsCamera : ICamera
{
    
    public Transform playerTarget;           // �÷��̾�
    public Transform cameraTransform;  // ī�޶�
    public Vector3 offset = new Vector3(0, 3, -5); // �þ� ��ġ
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
            Debug.Log("3��Ī ī�޶� Ȱ��ȭ");
        }
    }

    public void DeactivateCam()
    {
        if (cameraTransform != null)
        {
            Debug.Log("3��Ī ī�޶� ��Ȱ��ȭ");
        }
    }

   

    public void UpdateCam(Transform playerTarget)
    {
        if (playerTarget == null || cameraTransform == null) return;

       
        // ȸ��
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -40f, 85f);

        // ��
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // ī�޶� ���
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 zoomedOffset = rotation * new Vector3(0, offset.y, -currentZoom);
        Vector3 desiredPosition = playerTarget.position + zoomedOffset;

        cameraTransform.position = desiredPosition;
        cameraTransform.LookAt(playerTarget.position);
    }
}
