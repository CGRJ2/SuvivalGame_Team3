using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public InventoryUI inventoryUI;

    public void Init()
    {
        base.SingletonInit();
    }
}

[System.Serializable]
public class InventoryUI
{
    public InventoryView inventoryView;
    public SlotToolTip tooltip;
    public DragSlotView dragSlotInstance;
}
