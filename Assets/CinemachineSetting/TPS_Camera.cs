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
        if (CameraManager.Instance == null) { Debug.LogWarning("ī�޶� �Ŵ��� Ŭ���� ����"); return; }

        CameraManager.Instance.tpsCameraGroup = this;


    }
    public void OnEnable()
    {
        if (PlayerManager.Instance == null) { Debug.LogWarning("�÷��̾� �Ŵ��� Ŭ���� ����"); return; }
        if (PlayerManager.Instance.instancePlayer == null) { Debug.LogWarning("�÷��̾� �ν��Ͻ� ����"); return; }

        if (pc == PlayerManager.Instance.instancePlayer) return;

        // ���� �ν��Ͻ� �÷��̾� ���� �޾ƿ���
        pc = PlayerManager.Instance.instancePlayer;
        pc.TPS_Cameras = TPS_Cameras;

        // TPS ī�޶� �ʱ�ȭ
        foreach (CinemachineVirtualCamera virtualCamera in TPS_Cameras)
        {
            virtualCamera.Follow = pc.View.TPSView_CameraFocusTransform;
            virtualCamera.LookAt = pc.View.TPSView_CameraFocusTransform;
        }
    }
}
