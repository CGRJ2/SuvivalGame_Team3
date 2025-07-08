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


    // 생성자 => slotCount만큼의 인벤토리 슬롯 추가
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

    // 내가 받는 수량만큼 인벤토리에 다 넣을 수 있는지 여부를 반환 ---- Prensenter에서 호출
    public bool CanAddItem(Item item, int count)
    {
        List<SlotData> nowTab = GetCurrentTabSlots(item.itemType);
        return CanAddToInven(nowTab, item, count);
    }

    // 수량 안 적으면 한 개 추가
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
            // 빈칸이라면
            if (currentTab[i].item == null)
            {
                // 그 칸에 쌓을 수 있는 개수 만큼남은 개수에서 빼기
                remainCount -= item.maxCount;
            }
            // 같은 종류의 아이템이 인벤토리에 있으면
            else if (currentTab[i].IsStackable(item))
            {
                // 그 칸에 쌓을 수 있는 개수 만큼남은 개수에서 빼기
                remainCount -= currentTab[i].GetStackableCount();
            }

            // 남은 개수가 0 이하라면 foreach 탈출 (=> 인벤토리에 아이템 개수만큼 전부 추가 가능하다는 뜻)
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
            // 빈칸이 있다면
            if (currentTab[i].item == null)
            {
                // 그 칸에 최대스택 수 보다 적거나 같은 양이 남았을 때 => 남은 수량 전부 넣기
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
            // 같은 종류의 아이템이 인벤토리에 있으면
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
                    // 그 칸에 쌓을 수 있는 개수 만큼 추가
                    currentTab[i].AddItem(stackableCount);
                    remainCount -= stackableCount;
                }
            }
            
            // 남은 개수가 0 이하라면 foreach 탈출 (=> 인벤토리에 모두 추가 완료)
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
        Debug.Log($"{item.itemName}의 인벤토리 내 개수 : {OwnedItemCount}");
        return OwnedItemCount;
    }



    public void RemoveItem(Item item, int count)
    {
        List<SlotData> thisItemTypeTab = GetCurrentTabSlots(item.itemType);

        int removableCount = GetItemCountInList(thisItemTypeTab, item);

        if (removableCount < count)
        {
            Debug.LogError("현재 보유 중인 아이템 총 개수보다 많은 양을 제거하려고 시도함!");
            return;
        }

        int remainCount = count;

        foreach (SlotData sd in thisItemTypeTab)
        {
            // 같은 아이템 발견
            if(sd.item == item)
            {
                // 개수가 모자랄 때
                if (sd.currentCount < remainCount)
                {
                    // 현재 슬롯이 들고 있는 만큼 제거할 수량에서 제외
                    remainCount -= sd.currentCount;

                    // 현재 슬롯이 들고 있는 모든 아이템 제거
                    sd.CleanSlotData();
                }
                // 개수가 충분하면
                else
                {
                    sd.RemoveItem(remainCount);
                    return;
                }
            }
        }

    }



    // 인벤토리 내 아이템 데이터 세이브 용도
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
        
        // 재료 탭 동기화
        for(int i = 0; i < loadedSDListData.ingredientSlots.Count; i++)
        {
            if (loadedSDListData.ingredientSlots[i] == "Empty")
            {
                ingredientSlots[i].item = null;
            }
            else if (loadedSDListData.ingredientSlots[i].Contains("설계도"))
            {
                ingredientSlots[i].item = Resources.Load<Item>($"ItemDatabase/99 Recipes/{loadedSDListData.ingredientSlots[i]}");
            }
            else
            {
                ingredientSlots[i].item = Resources.Load<Item>($"ItemDatabase/01 Ingredient_Item/{loadedSDListData.ingredientSlots[i]}");
            }

            //이 슬롯과 연결된 퀵슬롯이 있다면?
        }

        // 소비 탭 동기화
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

        // 장비 탭 동기화
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

        // 기능아이템 탭 동기화
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

        // 퀘스트아이템 탭 동기화
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