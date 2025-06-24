using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "포션"; // Inspector에서 아이템 이름 지정
    public float pickupMessageTime = 3f; // 메시지 표시 시간

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 아이템 습득 로직 (인벤토리 추가되면 사용)
            // InventorySystem.Instance.Add(itemName);  < 아마도?

            // 알림 UI 출력
            UIController.Instance.ShowCollectNotification($"{itemName}을(를) 획득했습니다!", pickupMessageTime);

            // 아이템 오브젝트 삭제(혹은 비활성화)
            Destroy(gameObject);
        }
    }
}
