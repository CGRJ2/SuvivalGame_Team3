using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineBrain cinemachineBrain;
    public TPS_Camera tpsCameraGroup;
    public SideView_Camera sideViewCamera;

    public bool activeSideView;

    public void Init()
    {
        base.SingletonInit();
    }

    public void Start()
    {
        PlayerController pc = PlayerManager.Instance.instancePlayer;
        pc.TPS_Cameras = tpsCameraGroup.TPS_Cameras;
    }


    public void SwitchSideViewCamera(bool active)
    {
        if (active)
        {
            activeSideView = true;
            sideViewCamera.virtualCamera.Priority = 99;
        }
        else
        {
            activeSideView = false;
            sideViewCamera.virtualCamera.Priority = 0;
        }
    }


}