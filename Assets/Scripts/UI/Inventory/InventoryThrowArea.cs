using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryThrowArea : MonoBehaviour, IDropHandler
{

    // 아이템 버리기 영역
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryGroup.dragSlotInstance;

        // 퀵슬롯을 영역 밖으로 빼면 -> 퀵슬롯 해제
        if (dragSlotInstance.slotView is QuickSlot quick)
        {
            dragSlotInstance.slotView.ClearSlotView();

            quick.slotData = new SlotData();

            quick.SlotViewUpdate();
        }
        else if (dragSlotInstance.slotView != null)
        {
            if (dragSlotInstance.slotView.slotData == null) return;

            // 아이템 사용한 슬롯 상태 업데이트
            dragSlotInstance.slotView.slotData.CleanSlotData();
            dragSlotInstance.slotView.SlotViewUpdate();

            // 퀵슬롯 뷰 업데이트
            UIManager.Instance.inventoryGroup.quickSlotParent.UpdateQuickSlotView();
        }
    }
}
