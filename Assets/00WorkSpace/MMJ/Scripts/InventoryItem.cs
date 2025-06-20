using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;

    public InventoryItem(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public void ReduceQuantity(int amount)
    {
        quantity -= amount;
    }
}
