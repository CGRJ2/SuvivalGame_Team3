using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICamera
{
    
    Transform GetCameraTransform();

    void ActivateCam(Transform previousActiveCameraTransform);//카메라 활성화

    void DeactivateCam(); //카메라 비활성화

    void UpdateCam(Transform playerTarget);//LateUpdate에서 호출

    void InitializeCam(Transform playerTarget, Transform cameraTransform);// 초기 설정
}
