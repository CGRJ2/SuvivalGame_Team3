using UnityEngine;

public class SideViewTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 아니면 X
        if (other.GetComponent<PlayerController>() == null) return;

        // 화면 전환 중이면 return;
        if (CameraManager.Instance.cinemachineBrain.IsBlending) return;

        CameraManager.Instance.SwitchSideViewCamera(true);
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 아니면 X
        if (other.GetComponent<PlayerController>() == null) return;

        CameraManager.Instance.SwitchSideViewCamera(false);
    }
}
