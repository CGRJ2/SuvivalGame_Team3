using Cinemachine;
using UnityEngine;

public class SideViewTrigger : MonoBehaviour
{

    public Vector3 front;
    public Vector3 right;
    public Vector3 followOffset;


    private void OnTriggerEnter(Collider other)
    {
        CameraManager cm = CameraManager.Instance;
        // 플레이어가 아니면 X
        if (other.GetComponent<PlayerController>() == null) return;

        // 화면 전환 중이면 return;
        if (cm.cinemachineBrain.IsBlending) return;

        cm.SwitchSideViewCamera(true);

        cm.sideViewCamera.front = front;
        cm.sideViewCamera.right = right;
        cm.sideViewCamera.virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = followOffset;
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 아니면 X
        if (other.GetComponent<PlayerController>() == null) return;

        CameraManager.Instance.SwitchSideViewCamera(false);
    }
}
