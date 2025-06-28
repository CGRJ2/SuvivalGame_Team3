using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryThrowArea : MonoBehaviour, IDropHandler
{

    // ������ ������ ����
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        // �������� ���� ������ ���� -> ������ ����
        if (dragSlotInstance.slotView is QuickSlot quick)
        {
            dragSlotInstance.slotView.ClearSlotView();

            // ü�� ���¶�� => ü�� ����
            if (quick.chainedOriginSlotView != null)
            {
                quick.chainedOriginSlotView.chainedQuickSlot.slotData = new SlotData();
                quick.chainedOriginSlotView.chainedQuickSlot = null;
                quick.chainedOriginSlotView = null;
            }

            quick.SlotViewUpdate();
        }
        else if (dragSlotInstance.slotView != null)
        {
            if (dragSlotInstance.slotView.slotData == null) return;

            // ������ ����� ���� ���� ������Ʈ
            dragSlotInstance.slotView.slotData.CleanSlotData();
            dragSlotInstance.slotView.SlotViewUpdate();

            // ������ �� ������Ʈ
            UIManager.Instance.inventoryUI.quickSlotParent.UpdateQuickSlotView();
        }
    }
}
