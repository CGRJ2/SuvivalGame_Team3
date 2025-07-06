/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    
    public string targetCameraName;
    public string revertCameraName;
    public CameraManager cameraManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cameraManager != null && !string.IsNullOrEmpty(targetCameraName))
            {
                cameraManager.SwitchCamera(targetCameraName);
            }
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && cameraManager != null && !string.IsNullOrEmpty(revertCameraName))
        {
            cameraManager.SwitchCamera(revertCameraName);
        }
    }
}

*/