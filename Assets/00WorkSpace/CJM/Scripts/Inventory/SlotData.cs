using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class SlotData
{
    public Item item;
    public int currentCount = 0;
    public int maxCount;

    // 빈 슬롯 판정 용 기본 생성자
    public SlotData() { }

    // 빈 슬롯에 새로운 아이템을 추가할 때 => 생성자 사용
    public SlotData(Item item, int count, int maxCount)
    {
        this.item = item;
        this.currentCount += count;
        this.maxCount = maxCount;
    }

    public void CleanSlotData()
    {
        this.item = null;
        currentCount = 0;
        maxCount = 0;
    }

    public void AddItem(Item item, int count)  // 빈 슬롯에 아이템을 추가할 때
    {
        this.item = item;
        this.currentCount += count;
        this.maxCount = item.maxCount;
    }

    public void AddItem(int count)  // 슬롯에 이미 존재하는 아이템과 같은 아이템을 추가할 때
    {
        this.currentCount += count;
    }
    


    public void RemoveItem(int count)
    {
        this.currentCount -= count;
        if (currentCount == 0) CleanSlotData();
    }

    public bool IsStackable(Item item)
    {
        return this.item == item;
    }

    public int GetStackableCount()
    {
        return maxCount - currentCount;
    }



}
