using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIGroup : UIGroup
{
    public InventoryView inventoryView;
    public SlotToolTip tooltip;
    public DragSlotView dragSlotInstance;
    public QuickSlotParent quickSlotParent;
    public Panel_PlayerStatus panel_PlayerStatus;

    private void Awake() => Init();
    private void Start() => LateInit();
    public void Init()
    {
        panel_PlayerStatus.Init();
    }

    public void LateInit()
    {
    }
}
