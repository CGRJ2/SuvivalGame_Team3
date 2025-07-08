using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryModel
{
    [SerializeField] List<SlotData> ingredientSlots = new List<SlotData>();
    [SerializeField] List<SlotData> consumableSlots = new List<SlotData>();
    [SerializeField] List<SlotData> equipmentSlots = new List<SlotData>();
    [SerializeField] List<SlotData> functionSlots = new List<SlotData>();
    [SerializeField] List<SlotData> questSlots = new List<SlotData>();

    [SerializeField] int slotCount = 30;


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
        List<SlotData> thisItemTypeSlots = GetCurrentTabSlots(item.itemType);
        return GetItemCountInList(thisItemTypeSlots, item);
    }

    private int GetItemCountInList(List<SlotData> slotDataList, Item item)
    {
        int OwnedItemCount = 0;
        foreach (SlotData slotData in slotDataList)
        {
            if (slotData.item == item) OwnedItemCount += slotData.currentCount;
        }
        Debug.Log($"{item.itemName}�� �κ��丮 �� ���� : {OwnedItemCount}");
        return OwnedItemCount;
    }



    public void RemoveItem(Item item, int count)
    {
        List<SlotData> thisItemTypeTab = GetCurrentTabSlots(item.itemType);

        int removableCount = GetItemCountInList(thisItemTypeTab, item);

        if (removableCount < count)
        {
            Debug.LogError("���� ���� ���� ������ �� �������� ���� ���� �����Ϸ��� �õ���!");
            return;
        }

        int remainCount = count;

        foreach (SlotData sd in thisItemTypeTab)
        {
            // ���� ������ �߰�
            if(sd.item == item)
            {
                // ������ ���ڶ� ��
                if (sd.currentCount < remainCount)
                {
                    // ���� ������ ��� �ִ� ��ŭ ������ �������� ����
                    remainCount -= sd.currentCount;

                    // ���� ������ ��� �ִ� ��� ������ ����
                    sd.CleanSlotData();
                }
                // ������ ����ϸ�
                else
                {
                    sd.RemoveItem(remainCount);
                    return;
                }
            }
        }

    }



    // �κ��丮 �� ������ ������ ���̺� �뵵
    private string GetSlotItemKey(Item item)
    {
        if (item != null) return item.itemName;
        else return "Empty";
    }

    private List<string> GetItemDatasByKey(List<SlotData> currentTab)
    {
        List<string> keyDatas = new List<string>();
        foreach(SlotData slotData in currentTab)
        {
            keyDatas.Add(GetSlotItemKey(slotData.item));
        }
        return keyDatas;
    }


    public SlotDataListsData SaveSlotItemData()
    {
        SlotDataListsData slotDataListsData = new SlotDataListsData()
        {
            ingredientSlots = GetItemDatasByKey(ingredientSlots),
            consumableSlots = GetItemDatasByKey(consumableSlots),
            equipmentSlots = GetItemDatasByKey(equipmentSlots),
            functionSlots = GetItemDatasByKey(functionSlots),
            questSlots = GetItemDatasByKey(questSlots)
        };

        return slotDataListsData;
    }

    public void LoadSlotData(SaveDataGroup saveDataGroup)
    {
        SlotDataListsData loadedSDListData = saveDataGroup.slotDataListsData;
        
        // ��� �� ����ȭ
        for(int i = 0; i < loadedSDListData.ingredientSlots.Count; i++)
        {
            if (loadedSDListData.ingredientSlots[i] == "Empty")
            {
                ingredientSlots[i].item = null;
            }
            else if (loadedSDListData.ingredientSlots[i].Contains("���赵"))
            {
                ingredientSlots[i].item = Resources.Load<Item>($"ItemDatabase/99 Recipes/{loadedSDListData.ingredientSlots[i]}");
            }
            else
            {
                ingredientSlots[i].item = Resources.Load<Item>($"ItemDatabase/01 Ingredient_Item/{loadedSDListData.ingredientSlots[i]}");
            }

            //�� ���԰� ����� �������� �ִٸ�?
        }

        // �Һ� �� ����ȭ
        for (int i = 0; i < loadedSDListData.consumableSlots.Count; i++)
        {
            if (loadedSDListData.consumableSlots[i] == "Empty")
            {
                consumableSlots[i].item = null;
            }
            else
            {
                consumableSlots[i].item = Resources.Load<Item>($"ItemDatabase/02 Consumable_Item/{loadedSDListData.consumableSlots[i]}");
            }
        }

        // ��� �� ����ȭ
        for (int i = 0; i < loadedSDListData.equipmentSlots.Count; i++)
        {
            if (loadedSDListData.equipmentSlots[i] == "Empty")
            {
                equipmentSlots[i].item = null;
            }
            else
            {
                equipmentSlots[i].item = Resources.Load<Item>($"ItemDatabase/03 Equipment_Item/{loadedSDListData.equipmentSlots[i]}");
            }
        }

        // ��ɾ����� �� ����ȭ
        for (int i = 0; i < loadedSDListData.functionSlots.Count; i++)
        {
            if (loadedSDListData.functionSlots[i] == "Empty")
            {
                functionSlots[i].item = null;
            }
            else
            {
                functionSlots[i].item = Resources.Load<Item>($"ItemDatabase/04 Function_Item/{loadedSDListData.functionSlots[i]}");
            }
        }

        // ����Ʈ������ �� ����ȭ
        for (int i = 0; i < loadedSDListData.questSlots.Count; i++)
        {
            if (loadedSDListData.questSlots[i] == "Empty")
            {
                questSlots[i].item = null;
            }
            else
            {
                questSlots[i].item = Resources.Load<Item>($"ItemDatabase/05 Quest_Item/{loadedSDListData.questSlots[i]}");
            }
        }

    }
}

[System.Serializable]
public class SlotDataListsData
{
    public List<string> ingredientSlots;
    public List<string> consumableSlots;
    public List<string> equipmentSlots;
    public List<string> functionSlots;
    public List<string> questSlots;
}