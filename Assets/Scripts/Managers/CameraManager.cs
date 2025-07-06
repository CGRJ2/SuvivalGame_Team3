using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public TPS_Camera tpsCameraGroup;
    public SideView_Camera sideViewCamera;

    public void Init()
    {
        base.SingletonInit();
    }

    public void Start()
    {
        PlayerController pc = PlayerManager.Instance.instancePlayer;
        pc.TPS_Cameras = tpsCameraGroup.TPS_Cameras;
    }

    public void SwitchSideViewCamera(bool sideCamOn)
    {
        if (sideCamOn)
        {
            sideViewCamera.virtualCamera.gameObject.SetActive(true);
            sideViewCamera.virtualCamera.gameObject.transform.rotation = tpsCameraGroup.GetActivedCameraTransform().rotation;
        }
        else
        {
            sideViewCamera.virtualCamera.gameObject.SetActive(false);
            tpsCameraGroup.GetActivedCameraTransform().rotation = sideViewCamera.virtualCamera.gameObject.transform.rotation;
        }
    }

}