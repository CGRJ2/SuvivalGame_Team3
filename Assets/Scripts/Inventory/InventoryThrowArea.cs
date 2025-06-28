using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryThrowArea : MonoBehaviour, IDropHandler
{

    // 아이템 버리기 영역
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        // 퀵슬롯을 영역 밖으로 빼면 -> 퀵슬롯 해제
        if (dragSlotInstance.slot is QuickSlot quick)
        {
            dragSlotInstance.slot.ClearSlotView();

            // 체인 상태라면 => 체인 해제
            if (quick.chainedOriginSlotView != null)
            {
                quick.chainedOriginSlotView.chainedQuickSlot.slotData = null;
                quick.chainedOriginSlotView.chainedQuickSlot = null;
                quick.chainedOriginSlotView = null;
            }
        }
        else if (dragSlotInstance.slot != null)
        {
            if (dragSlotInstance.slot.slotData == null) return;

            // 아이템 사용한 슬롯 상태 업데이트
            dragSlotInstance.slot.slotData.CleanSlotData();
            dragSlotInstance.slot.SlotViewUpdate();

            // 퀵슬롯 뷰 업데이트
            UIManager.Instance.inventoryUI.quickSlotParent.UpdateQuickSlotView();
        }
    }
}
