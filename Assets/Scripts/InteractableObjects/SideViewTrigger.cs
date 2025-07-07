using UnityEngine;

public class SideViewTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ �ƴϸ� X
        if (other.GetComponent<PlayerController>() == null) return;

        // ȭ�� ��ȯ ���̸� return;
        if (CameraManager.Instance.cinemachineBrain.IsBlending) return;

        CameraManager.Instance.SwitchSideViewCamera(true);
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ �ƴϸ� X
        if (other.GetComponent<PlayerController>() == null) return;

        CameraManager.Instance.SwitchSideViewCamera(false);
    }
}
