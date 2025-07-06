using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideViewTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ �ƴϸ� X
        if (other.GetComponent<PlayerController>() == null) return;

        SideView_Camera sideViewCam = CameraManager.Instance.sideViewCamera;

        // �̹� SideView ���������� �ѱ�
        if (!sideViewCam.virtualCamera.gameObject.activeSelf)
            //sideViewCam.virtualCamera.gameObject.SetActive(true);
            CameraManager.Instance.SwitchSideViewCamera(true);
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ �ƴϸ� X
        if (other.GetComponent<PlayerController>() == null) return;

        SideView_Camera sideViewCam = CameraManager.Instance.sideViewCamera;

        // �̹� SideView ���������� ����
        if (sideViewCam.virtualCamera.gameObject.activeSelf)
            //sideViewCam.virtualCamera.gameObject.SetActive(false);
            CameraManager.Instance.SwitchSideViewCamera(false);
    }
}
