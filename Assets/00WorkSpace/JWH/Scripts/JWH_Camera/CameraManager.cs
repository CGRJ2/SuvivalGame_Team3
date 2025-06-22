using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerTarget;    // 플레이어 Transform 
    public Transform mainCameraTransform; // Main Camera의 Transform

    public TpsCamera tpsCamera;
    public SideCamera sideViewCamera;

    private Dictionary<string, ICamera> cameraStrategies = new Dictionary<string, ICamera>();
    private ICamera currentActiveCam;

    void Awake()
    {
        if (playerTarget == null)
        {
            Debug.LogError("CameraManager: Player Target 할당오류");
        }

        if (mainCameraTransform == null)
        {
            
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                mainCameraTransform = mainCam.transform;
            }
            else
            {
                Debug.LogError("CameraManager: Main Camera 할당오류");
                return;
            }
        }

        InitializeStrategies();
    }

    private void InitializeStrategies()
    {
       
        if (tpsCamera != null)
        {
            cameraStrategies.Add("TPS_View", tpsCamera);
            tpsCamera.InitializeCam(playerTarget, mainCameraTransform);
        }
        else
        {
            Debug.LogError("CameraManager: TPS Camera 할당오류");
        }

        if (sideViewCamera != null)
        {
            cameraStrategies.Add("Side_View", sideViewCamera);
            sideViewCamera.InitializeCam(playerTarget, mainCameraTransform);
        }
        else
        {
            Debug.LogError("CameraManager: Side Camera 할당오류");
        }

        
        if (tpsCamera != null)
        {
            SwitchCamera("TPS_View");
        }
        else
        {
            Debug.LogError("CameraManager: 기본 카메라 없음");
        }
    }

    void LateUpdate() 
    {
        if (currentActiveCam != null)
        {
            currentActiveCam.UpdateCam(playerTarget);
        }
    }

    
    public void SwitchCamera(string strategyName)
    {
        if (cameraStrategies.TryGetValue(strategyName, out ICamera targetStrategy))
        {
            if (currentActiveCam != null && currentActiveCam != targetStrategy)
            {
                currentActiveCam.DeactivateCam();
            }

            targetStrategy.ActivateCam(currentActiveCam?.GetCameraTransform());
            currentActiveCam = targetStrategy;
            Debug.Log($"CameraManager: 카메라가 '{strategyName}' 전환");
        }
        else
        {
            Debug.LogWarning($"CameraManager: '{strategyName}' 할당오류");
        }
    }

    
    public ICamera GetCurrentActiveStrategy()
    {
        return currentActiveCam;
    }
}