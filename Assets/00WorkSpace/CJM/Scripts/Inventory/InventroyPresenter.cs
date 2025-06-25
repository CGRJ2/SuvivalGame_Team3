using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventroyPresenter
{
    InventoryModel model = new InventoryModel(); // ==> DataField

    InventoryView view; // ==> UI

    public InventroyPresenter(InventoryModel model, InventoryView view)
    {
        this.model = model;
        this.view = view;
    }
    

    public void AddItem(Item item)
    {

    }
    public void UpdateUI()
    {

    }
}
