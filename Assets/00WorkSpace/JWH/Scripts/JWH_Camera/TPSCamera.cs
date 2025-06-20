using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsCamera : MonoBehaviour
{
    public Transform target;           // �÷��̾�
    public Transform cameraTransform;  // ī�޶�
    public Vector3 offset = new Vector3(0, 3, -5); // �þ� ��ġ
    public float rotationSpeed = 5f;

    private float yaw;
    private float pitch;
    private float currentZoom;

    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    void Start()
    {
        currentZoom = offset.magnitude;
    }

    void LateUpdate()
    {
        if (target == null) return;

        //ȸ��
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -40f, 85f);

        //��
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // ī�޶� ���
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 zoomedOffset = rotation * new Vector3(0, offset.y, -currentZoom);
        Vector3 desiredPosition = target.position + zoomedOffset;

        cameraTransform.position = desiredPosition;
        cameraTransform.LookAt(target.position);
    }
}