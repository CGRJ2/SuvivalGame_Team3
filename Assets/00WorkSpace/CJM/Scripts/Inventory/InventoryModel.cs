using System.Collections.Generic;

public class InventoryModel
{
    List<SlotData> recipeSlots = new List<SlotData>();
    List<SlotData> IngredientSlots = new List<SlotData>();
    List<SlotData> productSlots = new List<SlotData>();

    int slotCount;


    // ������ => slotCount��ŭ�� �κ��丮 ���� �߰�
    public InventoryModel()
    {
        for (int i = 0; i < slotCount; i++)
        {
            recipeSlots.Add(new SlotData());
            IngredientSlots.Add(new SlotData());
            productSlots.Add(new SlotData());
        }
    }

    public void AddItem(Item item)
    {
    }
}
