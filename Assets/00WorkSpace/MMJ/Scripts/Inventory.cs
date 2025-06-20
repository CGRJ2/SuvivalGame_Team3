using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxSlots = 20;

    public void AddItem(Item item)
    {
        InventoryItem existingItem = items.Find(i => i.item == item);

        if (existingItem != null && item.isStackable)
            existingItem.AddQuantity(1);
        else if (items.Count < maxSlots)
            items.Add(new InventoryItem(item, 1));
        else
            Debug.Log("인벤토리 꽉 참");
    }

    public void RemoveItem(Item item)
    {
        InventoryItem existingItem = items.Find(i => i.item == item);
        if (existingItem != null)
        {
            existingItem.ReduceQuantity(1);
            if (existingItem.quantity <= 0)
                items.Remove(existingItem);
        }
    }
}
