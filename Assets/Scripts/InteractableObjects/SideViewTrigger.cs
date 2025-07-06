using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideViewTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 아니면 X
        if (other.GetComponent<PlayerController>() == null) return;

        SideView_Camera sideViewCam = CameraManager.Instance.sideViewCamera;

        // 이미 SideView 꺼져있으면 켜기
        if (!sideViewCam.virtualCamera.gameObject.activeSelf)
            //sideViewCam.virtualCamera.gameObject.SetActive(true);
            CameraManager.Instance.SwitchSideViewCamera(true);
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 아니면 X
        if (other.GetComponent<PlayerController>() == null) return;

        SideView_Camera sideViewCam = CameraManager.Instance.sideViewCamera;

        // 이미 SideView 켜져있으면 끄기
        if (sideViewCam.virtualCamera.gameObject.activeSelf)
            //sideViewCam.virtualCamera.gameObject.SetActive(false);
            CameraManager.Instance.SwitchSideViewCamera(false);
    }
}
