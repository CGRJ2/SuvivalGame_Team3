using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    public InventoryUI inventoryUI;

    public CraftingUI craftingUI;

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
    public QuickSlotParent quickSlotParent;
}

[System.Serializable]
public class CraftingUI
{
    public CraftingUIGroup craftingUIGroup;
}
