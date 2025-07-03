using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class SlotData
{
    public Item item;
    public int currentCount = 0;
    public int maxCount;

    // �� ���� ���� �� �⺻ ������
    public SlotData() { }

    // �� ���Կ� ���ο� �������� �߰��� �� => ������ ���
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

    public void AddItem(Item item, int count)  // �� ���Կ� �������� �߰��� ��
    {
        this.item = item;
        this.currentCount += count;
        this.maxCount = item.maxCount;
    }

    public void AddItem(int count)  // ���Կ� �̹� �����ϴ� �����۰� ���� �������� �߰��� ��
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
