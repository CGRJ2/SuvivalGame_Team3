using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerTarget;    // �÷��̾� Transform 
    public Transform mainCameraTransform; // Main Camera�� Transform

    public TpsCamera tpsCamera;
    public SideCamera sideViewCamera;

    private Dictionary<string, ICamera> cameraStrategies = new Dictionary<string, ICamera>();
    private ICamera currentActiveCam;

    void Awake()
    {
        if (playerTarget == null)
        {
            Debug.LogError("CameraManager: Player Target �Ҵ����");
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
                Debug.LogError("CameraManager: Main Camera �Ҵ����");
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
            Debug.LogError("CameraManager: TPS Camera �Ҵ����");
        }

        if (sideViewCamera != null)
        {
            cameraStrategies.Add("Side_View", sideViewCamera);
            sideViewCamera.InitializeCam(playerTarget, mainCameraTransform);
        }
        else
        {
            Debug.LogError("CameraManager: Side Camera �Ҵ����");
        }

        
        if (tpsCamera != null)
        {
            SwitchCamera("TPS_View");
        }
        else
        {
            Debug.LogError("CameraManager: �⺻ ī�޶� ����");
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
            Debug.Log($"CameraManager: ī�޶� '{strategyName}' ��ȯ");
        }
        else
        {
            Debug.LogWarning($"CameraManager: '{strategyName}' �Ҵ����");
        }
    }

    
    public ICamera GetCurrentActiveStrategy()
    {
        return currentActiveCam;
    }
}