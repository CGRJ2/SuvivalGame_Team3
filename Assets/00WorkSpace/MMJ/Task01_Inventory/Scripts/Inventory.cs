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

    private ItemType currentTab = ItemType.Equipment; // �⺻ ���� ��

    private void Start()
    {
        equipmentSlots = go_EquipmentSlotsParent.GetComponentsInChildren<Slot>();
        usedSlots = go_UsedSlotsParent.GetComponentsInChildren<Slot>();
        ingredientSlots = go_IngredientSlotsParent.GetComponentsInChildren<Slot>();
        functionSlots = go_FunctionSlotsParent.GetComponentsInChildren<Slot>();
        questSlots = go_QuestSlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = go_QuickSlotsParent.GetComponentsInChildren<Slot>();

        ChangeTab(ItemType.Equipment); // �ʱ� ������
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
        Slot[] targetSlots = GetTargetSlotArray(_item.itemType); // �ٷ� Ÿ�Ժ� ��������
        PutSlot(targetSlots, _item, _count);

        if (isNotPut)
            Debug.Log("�κ��丮�� ��á���ϴ�!");
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
        if (ItemType.Equipment != _item.itemType) // ��� �ƴ� ��� ���� �õ�
        {
            for (int i = 0; i < _slots.Length; i++) //��� ���� ��ȸ
            {
                if (_slots[i].item != null && _slots[i].item.itemName == _item.itemName) //�̸��� ��ġ�Ѵٸ�
                {
                    _slots[i].SetSlotCount(_count); //�ش� ���� �������� ī��Ʈ�� 1 �߰�
                    isNotPut = false;
                    return;
                }
            }
        }
        // �� ���� ã��
        for (int i = 0; i < _slots.Length; i++) // ��� ���� ��ȸ
        {
            if (_slots[i].item == null) // �ش��ϴ� �̸��� ������
            {
                _slots[i].AddItem(_item, _count); // ������ �κ��丮�� �߰�
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

    public void CraftItem(CraftingRecipe recipe)  // ũ�������� ���� �׽�Ʈ�ڵ�
    {
        // ��� ����
        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            int remainToRemove = recipe.requiredCounts[i];
            Slot[] slots = GetTargetSlotArray(recipe.requiredItems[i].itemType);

            foreach (Slot slot in slots)
            {
                if (slot.item == recipe.requiredItems[i])
                {
                    int removed = slot.ReduceItem(remainToRemove); // �Ʒ� �Լ� ����
                    remainToRemove -= removed;

                    if (remainToRemove <= 0)
                        break;
                }
            }
        }

        // ����� �߰�
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