using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideView_Camera : MonoBehaviour
{
    PlayerController pc;
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 front;
    public Vector3 right;

    public void Awake()
    {
        if (CameraManager.Instance == null) { Debug.LogWarning("ī�޶� �Ŵ��� Ŭ���� ����"); return; }

        CameraManager.Instance.sideViewCamera = this;
    }

    private void OnEnable()
    {
        // �̹� �ʱ�ȭ �� �÷��̾�� �����ص���
        if (pc == PlayerManager.Instance.instancePlayer) return;

        // ���� �÷��̾� �ν��Ͻ� ������ ���� �ʱ�ȭ
        pc = PlayerManager.Instance.instancePlayer;

        // ���̵�� ī�޶� �ʱ�ȭ
        virtualCamera.Follow = pc.View.SideView_CameraFocusTransform;
        virtualCamera.LookAt = pc.View.SideView_CameraFocusTransform;
    }
}
