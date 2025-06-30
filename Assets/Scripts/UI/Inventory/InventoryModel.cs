using System.Collections.Generic;
using UnityEngine;
public class InventoryModel
{
    List<SlotData> ingredientSlots = new List<SlotData>();
    List<SlotData> consumableSlots = new List<SlotData>();
    List<SlotData> equipmentSlots = new List<SlotData>();
    List<SlotData> functionSlots = new List<SlotData>();
    List<SlotData> questSlots = new List<SlotData>();

    int slotCount = 30;


    // ������ => slotCount��ŭ�� �κ��丮 ���� �߰�
    public InventoryModel()
    {
        for (int i = 0; i < slotCount; i++)
        {
            ingredientSlots.Add(new SlotData());
            consumableSlots.Add(new SlotData());
            equipmentSlots.Add(new SlotData());
            functionSlots.Add(new SlotData());
            questSlots.Add(new SlotData());
        }
    }

    // 
    public List<SlotData> GetCurrentTabSlots(ItemType tabType)
    {
        List<SlotData> nowTab;
        switch (tabType)
        {
            case ItemType.Ingredient:
                nowTab = ingredientSlots;
                break;
            case ItemType.Consumalbe:
                nowTab = consumableSlots;
                break;
            case ItemType.Equipment:
                nowTab = equipmentSlots;
                break;
            case ItemType.Function:
                nowTab = functionSlots;
                break;
            case ItemType.Quest:
                nowTab = questSlots;
                break;
            default: nowTab = null; break;
        }

        return nowTab;
    }

    // ���� �޴� ������ŭ �κ��丮�� �� ���� �� �ִ��� ���θ� ��ȯ ---- Prensenter���� ȣ��
    public bool CanAddItem(Item item, int count)
    {
        List<SlotData> nowTab = GetCurrentTabSlots(item.itemType);
        return CanAddToInven(nowTab, item, count);
    }

    // ���� �� ������ �� �� �߰�
    public void AddItem(Item item, int count)
    {
        List<SlotData> nowTab = GetCurrentTabSlots(item.itemType);
        AddToInven(nowTab, item, count);
    }

    private bool CanAddToInven(List<SlotData> currentTab, Item item, int count)
    {
        int remainCount = count;
        bool canAdd = false;
        for (int i = 0; i < currentTab.Count; i ++) 
        {
            // ��ĭ�̶��
            if (currentTab[i].item == null)
            {
                // �� ĭ�� ���� �� �ִ� ���� ��ŭ���� �������� ����
                remainCount -= item.maxCount;
            }
            // ���� ������ �������� �κ��丮�� ������
            else if (currentTab[i].IsStackable(item))
            {
                // �� ĭ�� ���� �� �ִ� ���� ��ŭ���� �������� ����
                remainCount -= currentTab[i].GetStackableCount();
            }

            // ���� ������ 0 ���϶�� foreach Ż�� (=> �κ��丮�� ������ ������ŭ ���� �߰� �����ϴٴ� ��)
            if (remainCount <= 0)
            {
                canAdd = true;
                break;
            }
        }

        return canAdd;
    }

    private void AddToInven(List<SlotData> currentTab, Item item, int count)
    {
        int remainCount = count;
        int maxCount = item.maxCount;

        for (int i = 0; i < currentTab.Count; i ++) 
        {
            // ��ĭ�� �ִٸ�
            if (currentTab[i].item == null)
            {
                // �� ĭ�� �ִ뽺�� �� ���� ���ų� ���� ���� ������ �� => ���� ���� ���� �ֱ�
                if (maxCount >= remainCount)
                {
                    currentTab[i].AddItem(item, remainCount);
                    remainCount -= maxCount;
                }
                else
                {
                    currentTab[i].AddItem(item, maxCount);
                    remainCount -= maxCount;
                }
            }
            // ���� ������ �������� �κ��丮�� ������
            else if (currentTab[i].IsStackable(item))
            {
                int stackableCount = currentTab[i].GetStackableCount();

                if (remainCount < stackableCount)
                {
                    currentTab[i].AddItem(remainCount);
                    remainCount = 0;
                }
                else
                {
                    // �� ĭ�� ���� �� �ִ� ���� ��ŭ �߰�
                    currentTab[i].AddItem(stackableCount);
                    remainCount -= stackableCount;
                }
            }
            
            // ���� ������ 0 ���϶�� foreach Ż�� (=> �κ��丮�� ��� �߰� �Ϸ�)
            if (remainCount <= 0)
            {
                break;
            }
        }
    }

    public int GetOwnedItemCount(Item item)
    {
        int ownedItemCount = 0;
        switch (item.itemType)
        {
            case ItemType.Ingredient:
                ownedItemCount = GetItemCountInList(ingredientSlots, item);
                break;

            case ItemType.Consumalbe:
                ownedItemCount = GetItemCountInList(consumableSlots, item);
                break;

            case ItemType.Equipment:
                ownedItemCount = GetItemCountInList(equipmentSlots, item);
                break;

            case ItemType.Function:
                ownedItemCount = GetItemCountInList(functionSlots, item);

                break;
            case ItemType.Quest:
                ownedItemCount = GetItemCountInList(questSlots, item);
                break;

            default: break;
        }
        return ownedItemCount;
    }

    private int GetItemCountInList(List<SlotData> slotDataList, Item item)
    {
        int OwnedItemCount = 0;
        foreach (SlotData slotData in slotDataList)
        {
            if (slotData.item = item) OwnedItemCount += slotData.currentCount;
        }
        return OwnedItemCount;
    }

}
