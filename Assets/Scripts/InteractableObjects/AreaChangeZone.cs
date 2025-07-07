using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChangeZone : MonoBehaviour
{

    [SerializeField] RegionInfos thisSideRegionInfo;
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 아니면 X
        if (other.GetComponent<PlayerController>() == null) return;

        // 이미 이 지역이면 X
        if (StageManager.Instance.nowRegion == thisSideRegionInfo) return;


        StageManager.Instance.nowRegion = thisSideRegionInfo;

        string message;
        switch (thisSideRegionInfo)
        {
            case RegionInfos.LivingRoom:   message = "거실"; break;
            case RegionInfos.Library:      message = "서재"; break;
            case RegionInfos.DressRoom:    message = "옷방"; break;
            case RegionInfos.MasterBedRoom:message = "안방"; break;
            case RegionInfos.CatRoom:      message = "고양이방"; break;
            case RegionInfos.TutorialRoom: message = "아이방"; break;
            case RegionInfos.BaseCamp: message = "베이스 캠프"; break;
            default: message = "방 정보 없음!"; break;
        }

        Panel_RoomInfo roomInfo = UIManager.Instance.popUpUIGroup.RoomInfoUI;

        UIManager.Instance.popUpUIGroup.RoomInfoUI.PopMessage_FadeInOut(message);
    }
}
