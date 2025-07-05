using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChangeZone : MonoBehaviour
{

    [SerializeField] RegionInfos thisSideRegionInfo;
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ �ƴϸ� X
        if (other.GetComponent<PlayerController>() == null) return;

        // �̹� �� �����̸� X
        if (StageManager.Instance.nowRegion == thisSideRegionInfo) return;


        StageManager.Instance.nowRegion = thisSideRegionInfo;

        string message;
        switch (thisSideRegionInfo)
        {
            case RegionInfos.LivingRoom:   message = "�Ž�"; break;
            case RegionInfos.Library:      message = "����"; break;
            case RegionInfos.DressRoom:    message = "�ʹ�"; break;
            case RegionInfos.MasterBedRoom:message = "�ȹ�"; break;
            case RegionInfos.CatRoom:      message = "����̹�"; break;
            case RegionInfos.TutorialRoom: message = "���̹�"; break;
            case RegionInfos.BaseCamp: message = "���̽� ķ��"; break;
            default: message = "�� ���� ����!"; break;
        }

        Panel_RoomInfo roomInfo = UIManager.Instance.popUpUIGroup.RoomInfoUI;

        UIManager.Instance.popUpUIGroup.RoomInfoUI.PopMessage_FadeInOut(message);
    }
}
