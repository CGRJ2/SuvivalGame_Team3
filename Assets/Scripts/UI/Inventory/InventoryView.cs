using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryView : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject go_inventoryBase;

    [SerializeField] private GameObject itemSlots;

    private SlotView[] slots;


    public ObservableProperty<ItemType> CurrentTab = new ObservableProperty<ItemType>();

    private void Start()
    {
        slots = itemSlots.GetComponentsInChildren<SlotView>();

        // �⺻ ���� ������ ����
        CurrentTab.Value = ItemType.Equipment;
    }

    public void UpdateInventorySlotView(List<SlotData> slotDatas)
    {
        // ������ �־��ֱ�
        for (int i = 0; i < slotDatas.Count; i++)
        {
            //slots[i].slotData �̰Ŵ� ���� ����. SlotView�� �پ��ִ� slotData�� ������ �ٷ� ������ �ʵ��ε�,, 
            slots[i].slotData = slotDatas[i];
            
            // ��� ���� �� ������Ʈ
            slots[i].SlotViewUpdate();
        }

        // �����Ե鵵 ������Ʈ
        UIManager.Instance.inventoryGroup.quickSlotParent.UpdateQuickSlotView();
    }

    private void OnDestroy()
    {
        CurrentTab.UnsbscribeAll();
    }

    public void TryOpenInventory()
    {
        if (!inventoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        inventoryActivated = true;
        UIManager.Instance.OpenPanelNotChangeActionMap(go_inventoryBase);
    }

    private void CloseInventory()
    {
        inventoryActivated = false;
        UIManager.Instance.CloseTargetPanel(go_inventoryBase);
    }

    public void OnClickEquipmentTab() => CurrentTab.Value = ItemType.Equipment;
    public void OnClickConsuableTab() => CurrentTab.Value = ItemType.Consumalbe;
    public void OnClickIngredient() => CurrentTab.Value = ItemType.Ingredient;
    public void OnClickFunctionTab() => CurrentTab.Value = ItemType.Function;
    public void OnClickQuestTab() => CurrentTab.Value = ItemType.Quest;


    
}
