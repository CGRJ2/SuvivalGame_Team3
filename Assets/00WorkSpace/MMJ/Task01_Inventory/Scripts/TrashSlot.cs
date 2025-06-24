using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            // 실제 버리는 처리
            Debug.Log(DragSlot.instance.dragSlot.item.itemName + " 아이템을 버렸습니다.");

            // 드래그하던 슬롯 비우기
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
