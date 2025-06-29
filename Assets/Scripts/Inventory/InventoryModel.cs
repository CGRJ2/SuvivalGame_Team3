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
}
