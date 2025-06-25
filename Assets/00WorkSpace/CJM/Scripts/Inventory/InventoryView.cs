using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject go_inventoryBase;

    [SerializeField] private GameObject itemSlots;

    private SlotView[] slots;
  

    public ItemType currentTab; // 기본 무기 탭

    private void Start()
    {
        UIManager.Instance.inventoryUI.inventoryView.Value = this;

        slots = itemSlots.GetComponentsInChildren<SlotView>();

        currentTab = ItemType.Equipment;
    }

    public void UpdateInventorySlotView(List<SlotData> slotDatas)
    {
        // 데이터 넣어주기
        for (int i = 0; i < slotDatas.Count; i++)
        {
            slots[i].slotData = slotDatas[i];
            slots[i].SlotViewUpdate();
        }
    }



    public void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;

        if (inventoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        go_inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_inventoryBase.SetActive(false);
    }

    public void OnClickEquipmentTab() => currentTab = ItemType.Equipment;
    public void OnClickConsuableTab() => currentTab = ItemType.Used;
    public void OnClickIngredient() => currentTab = ItemType.Ingredient;
    public void OnClickFunctionTab() => currentTab = ItemType.Function;
    public void OnClickQuestTab() => currentTab = ItemType.Quest;
}
