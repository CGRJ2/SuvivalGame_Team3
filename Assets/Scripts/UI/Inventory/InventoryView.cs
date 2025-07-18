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

        // 기본 무기 탭으로 설정
        CurrentTab.Value = ItemType.Equipment;
    }

    public void UpdateInventorySlotView(List<SlotData> slotDatas)
    {
        // 데이터 넣어주기
        for (int i = 0; i < slotDatas.Count; i++)
        {
            //slots[i].slotData 이거는 원본 맞음. SlotView에 붙어있는 slotData는 원본을 바로 참조한 필드인데,, 
            slots[i].slotData = slotDatas[i];
            
            // 모든 슬롯 뷰 업데이트
            slots[i].SlotViewUpdate();
        }

        // 퀵슬롯들도 업데이트
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
