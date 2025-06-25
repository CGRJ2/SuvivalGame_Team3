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
    public ObservableProperty<InventoryView> inventoryView = new ObservableProperty<InventoryView>();
    public SlotToolTip tooltip;
}
