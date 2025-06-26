using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject go_inventoryBase;

    [SerializeField] private GameObject go_EquipmentSlotsParent;
    [SerializeField] private GameObject go_UsedSlotsParent;
    [SerializeField] private GameObject go_IngredientSlotsParent;
    [SerializeField] private GameObject go_FunctionSlotsParent;
    [SerializeField] private GameObject go_QuestSlotsParent; 
    [SerializeField] private GameObject go_QuickSlotsParent;

    [SerializeField] private GameObject go_Base; // Tooltip Base_Outer

    private Slot[] equipmentSlots;
    private Slot[] usedSlots;
    private Slot[] ingredientSlots;
    private Slot[] functionSlots;
    private Slot[] questSlots;
    private Slot[] quickSlots;

    private bool isNotPut;

    private ItemType currentTab = ItemType.Equipment; // 기본 무기 탭

    private void Start()
    {
        equipmentSlots = go_EquipmentSlotsParent.GetComponentsInChildren<Slot>();
        usedSlots = go_UsedSlotsParent.GetComponentsInChildren<Slot>();
        ingredientSlots = go_IngredientSlotsParent.GetComponentsInChildren<Slot>();
        functionSlots = go_FunctionSlotsParent.GetComponentsInChildren<Slot>();
        questSlots = go_QuestSlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = go_QuickSlotsParent.GetComponentsInChildren<Slot>();

        ChangeTab(ItemType.Equipment); // 초기 무기탭
    }

    private void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        go_inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_inventoryBase.SetActive(false);
        go_Base.SetActive(false);
    }

    public void ChangeTab(ItemType type)
    {
        currentTab = type;

        go_EquipmentSlotsParent.SetActive(false);
        go_UsedSlotsParent.SetActive(false);
        go_IngredientSlotsParent.SetActive(false);
        go_FunctionSlotsParent.SetActive(false);
        go_QuestSlotsParent.SetActive(false);

        switch (currentTab)
        {
            case ItemType.Equipment: go_EquipmentSlotsParent.SetActive(true); break;
            case ItemType.Used: go_UsedSlotsParent.SetActive(true); break;
            case ItemType.Ingredient: go_IngredientSlotsParent.SetActive(true); break;
            case ItemType.Function: go_FunctionSlotsParent.SetActive(true); break;
            case ItemType.Quest: go_QuestSlotsParent.SetActive(true); break;
        }
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        Slot[] targetSlots = GetTargetSlotArray(_item.itemType); // 바로 타입별 슬롯으로
        PutSlot(targetSlots, _item, _count);

        if (isNotPut)
            Debug.Log("인벤토리가 꽉찼습니다!");
    }

    private Slot[] GetTargetSlotArray(ItemType type)
    {
        switch (type)
        {
            case ItemType.Equipment: return equipmentSlots;
            case ItemType.Used: return usedSlots;
            case ItemType.Ingredient: return ingredientSlots;
            case ItemType.Function: return functionSlots;
            case ItemType.Quest: return questSlots;
        }
        return null;
    }

    private void PutSlot(Slot[] _slots, Item _item, int _count)
    {
        if (ItemType.Equipment != _item.itemType) // 장비 아닌 경우 스택 시도
        {
            for (int i = 0; i < _slots.Length; i++) //모든 슬롯 순회
            {
                if (_slots[i].item != null && _slots[i].item.itemName == _item.itemName) //이름이 일치한다면
                {
                    _slots[i].SetSlotCount(_count); //해당 슬롯 아이템의 카운트를 1 추가
                    isNotPut = false;
                    return;
                }
            }
        }
        // 빈 슬롯 찾기
        for (int i = 0; i < _slots.Length; i++) // 모든 슬롯 순회
        {
            if (_slots[i].item == null) // 해당하는 이름이 없으면
            {
                _slots[i].AddItem(_item, _count); // 아이템 인벤토리에 추가
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }

    public void OnClickEquipmentTab() => ChangeTab(ItemType.Equipment);
    public void OnClickUsedTab() => ChangeTab(ItemType.Used);
    public void OnClickIngredient() => ChangeTab(ItemType.Ingredient);
    public void OnClickFunctionTab() => ChangeTab(ItemType.Function);
    public void OnClickQuestTab() => ChangeTab(ItemType.Quest);


    public bool HasRequiredItems(CraftingRecipe recipe)
    {
        Slot[] ingredientSlots = GetTargetSlotArray(ItemType.Ingredient);

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            int requiredCount = recipe.requiredCounts[i];
            int totalCount = 0;

            foreach (Slot slot in ingredientSlots)
            {
                if (slot.item != null && slot.item.itemName == recipe.requiredItems[i].itemName)
                {
                    totalCount += slot.itemCount;
                }
            }

            if (totalCount < requiredCount)
                return false;
        }

        return true;
    }

    public void CraftItem(CraftingRecipe recipe)  // 크래프팅을 위한 테스트코드
    {
        // 재료 차감
        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            int remainToRemove = recipe.requiredCounts[i];
            Slot[] slots = GetTargetSlotArray(recipe.requiredItems[i].itemType);

            foreach (Slot slot in slots)
            {
                if (slot.item == recipe.requiredItems[i])
                {
                    int removed = slot.ReduceItem(remainToRemove); // 아래 함수 참고
                    remainToRemove -= removed;

                    if (remainToRemove <= 0)
                        break;
                }
            }
        }

        // 결과물 추가
        AcquireItem(recipe.resultItem, recipe.resultCount);
    }
    public int GetItemCount(Item item)
    {
        int count = 0;
        Slot[] slots = GetTargetSlotArray(item.itemType);

        foreach (Slot slot in slots)
        {
            if (slot.item != null && slot.item == item)
                count += slot.itemCount;
        }

        return count;
    }

}