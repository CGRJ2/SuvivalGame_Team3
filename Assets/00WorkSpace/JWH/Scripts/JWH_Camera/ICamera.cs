using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICamera
{
    
    Transform GetCameraTransform();

    void ActivateCam(Transform previousActiveCameraTransform);//ī�޶� Ȱ��ȭ

    void DeactivateCam(); //ī�޶� ��Ȱ��ȭ

    void UpdateCam(Transform playerTarget);//LateUpdate���� ȣ��

    void InitializeCam(Transform playerTarget, Transform cameraTransform);// �ʱ� ����
}
