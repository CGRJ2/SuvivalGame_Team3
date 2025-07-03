using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryThrowArea : MonoBehaviour, IDropHandler
{

    // ������ ������ ����
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryGroup.dragSlotInstance;

        // �������� ���� ������ ���� -> ������ ����
        if (dragSlotInstance.slotView is QuickSlot quick)
        {
            dragSlotInstance.slotView.ClearSlotView();

            quick.slotData = new SlotData();

            quick.SlotViewUpdate();
        }
        else if (dragSlotInstance.slotView != null)
        {
            if (dragSlotInstance.slotView.slotData == null) return;

            // ������ ����� ���� ���� ������Ʈ
            dragSlotInstance.slotView.slotData.CleanSlotData();
            dragSlotInstance.slotView.SlotViewUpdate();

            // ������ �� ������Ʈ
            UIManager.Instance.inventoryGroup.quickSlotParent.UpdateQuickSlotView();
        }
    }
}
