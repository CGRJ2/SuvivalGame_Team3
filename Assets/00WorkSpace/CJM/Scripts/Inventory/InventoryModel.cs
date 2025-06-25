using System.Collections.Generic;

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
            ingredientSlots.Add(new SlotData(30));
            consumableSlots.Add(new SlotData(10));
            equipmentSlots.Add(new SlotData(1));
            functionSlots.Add(new SlotData(1));
            questSlots.Add(new SlotData(1));
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
            case ItemType.Used:
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

        foreach(SlotData sd in currentTab)
        {
            // ���� ������ �������� �κ��丮�� ������
            if (sd.IsStackable(item))
            {
                // �� ĭ�� ���� �� �ִ� ���� ��ŭ���� �������� ����
                remainCount -= sd.GetStackableCount();
            }
            // ��ĭ�� �ִٸ�
            else if (sd.IsSlotEmpty())
            {
                // �� ĭ�� ���� �� �ִ� ���� ��ŭ���� �������� ����
                remainCount -= sd.maxCount;
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

        foreach (SlotData sd in currentTab)
        {
            // ���� ������ �������� �κ��丮�� ������
            if (sd.IsStackable(item))
            {
                // �� ĭ�� ���� �� �ִ� ���� ��ŭ �߰�
                sd.AddItem(sd.GetStackableCount());

                // ���� ��ŭ ���� ���� ����
                remainCount -= sd.GetStackableCount();
            }
            // ��ĭ�� �ִٸ�
            else if (sd.IsSlotEmpty())
            {
                // �� ĭ�� �ִ뽺�� �� ���� ���ų� ���� ���� ������ �� => ���� ���� ���� �ֱ�
                if (sd.maxCount >= remainCount)
                {
                    sd.AddItem(item, remainCount);
                    remainCount -= sd.maxCount;
                }
                else
                {
                    sd.AddItem(item, sd.maxCount);
                    remainCount -= sd.maxCount;
                }
            }
            // ���� ������ 0 ���϶�� foreach Ż�� (=> �κ��丮�� ��� �߰� �Ϸ�)
            if (remainCount <= 0)
            {
                break;
            }
        }
    }
}
