using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotData
{
    public Item item;
    public int currentCount = 0;
    public int maxCount;

    public SlotData(int maxCount)
    {
        this.maxCount = maxCount;
    }

    public void CleanSlot()
    {
        this.item = null;
        currentCount = 0;
    }

    public void AddItem(Item item, int count)  // 빈 슬롯에 새로운 아이템을 추가할 때
    {
        this.item = item;
        this.currentCount += count;
    }
    public void AddItem(int count)  // 슬롯에 이미 존재하는 아이템과 같은 아이템을 추가할 때
    {
        this.currentCount += count;
    }

    public void RemoveItem(int count)
    {
        this.currentCount -= count;
        if (currentCount == 0) CleanSlot();
    }

    public bool IsStackable(Item item)
    {
        return this.item == item;
    }

    public int GetStackableCount()
    {
        return maxCount - currentCount;
    }

    public bool IsSlotEmpty()
    {
        return item == null;
    }
    
}
