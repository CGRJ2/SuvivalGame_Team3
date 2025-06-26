using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject go_inventoryBase;

    [SerializeField] private GameObject itemSlots;

    private SlotView[] slots;


    public ObservableProperty<ItemType> CurrentTab = new ObservableProperty<ItemType>();

    private void Start()
    {
        UIManager.Instance.inventoryUI.inventoryView.Value = this;

        slots = itemSlots.GetComponentsInChildren<SlotView>();

        // 기본 무기 탭으로 설정
        CurrentTab.Value = ItemType.Equipment;
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

    private void OnDestroy()
    {
        CurrentTab.UnsbscribeAll();
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

    public void OnClickEquipmentTab() => CurrentTab.Value = ItemType.Equipment;
    public void OnClickConsuableTab() => CurrentTab.Value = ItemType.Used;
    public void OnClickIngredient() => CurrentTab.Value = ItemType.Ingredient;
    public void OnClickFunctionTab() => CurrentTab.Value = ItemType.Function;
    public void OnClickQuestTab() => CurrentTab.Value = ItemType.Quest;
}
