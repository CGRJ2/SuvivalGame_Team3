using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideView_Camera : MonoBehaviour
{
    PlayerController pc;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public void Awake()
    {
        if (CameraManager.Instance == null) { Debug.LogWarning("카메라 매니저 클래스 없음"); return; }

        CameraManager.Instance.sideViewCamera = this;
    }

    private void OnEnable()
    {
        // 이미 초기화 된 플레이어면 무시해도됨
        if (pc == PlayerManager.Instance.instancePlayer) return;

        // 현재 플레이어 인스턴스 정보에 맞춰 초기화
        pc = PlayerManager.Instance.instancePlayer;

        // 사이드뷰 카메라 초기화
        virtualCamera.Follow = pc.View.TPSView_CameraFocusTransform;
        virtualCamera.LookAt = pc.View.TPSView_CameraFocusTransform;
    }
}
