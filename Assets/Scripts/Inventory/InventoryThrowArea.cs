using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryThrowArea : MonoBehaviour, IDropHandler
{

    // ������ ������ ����
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        // �������� ���� ������ ���� -> ������ ����
        if (dragSlotInstance.slot is QuickSlot quick)
        {
            dragSlotInstance.slot.ClearSlotView();

            // ü�� ���¶�� => ü�� ����
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

            // ������ ����� ���� ���� ������Ʈ
            dragSlotInstance.slot.slotData.CleanSlotData();
            dragSlotInstance.slot.SlotViewUpdate();

            // ������ �� ������Ʈ
            UIManager.Instance.inventoryUI.quickSlotParent.UpdateQuickSlotView();
        }
    }
}
