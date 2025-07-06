using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour
{
    public CinemachineVirtualCamera[] TPS_Cameras;
    PlayerController pc;

    public void Awake()
    {
        if (CameraManager.Instance == null) { Debug.LogWarning("카메라 매니저 클래스 없음"); return; }

        CameraManager.Instance.tpsCameraGroup = this;


    }
    public void OnEnable()
    {
        if (PlayerManager.Instance == null) { Debug.LogWarning("플레이어 매니저 클래스 없음"); return; }
        if (PlayerManager.Instance.instancePlayer == null) { Debug.LogWarning("플레이어 인스턴스 없음"); return; }

        if (pc == PlayerManager.Instance.instancePlayer) return;

        // 현재 인스턴스 플레이어 정보 받아오기
        pc = PlayerManager.Instance.instancePlayer;
        pc.TPS_Cameras = TPS_Cameras;

        // TPS 카메라 초기화
        foreach (CinemachineVirtualCamera virtualCamera in TPS_Cameras)
        {
            virtualCamera.Follow = pc.View.TPSView_CameraFocusTransform;
            virtualCamera.LookAt = pc.View.TPSView_CameraFocusTransform;
        }
    }
}
