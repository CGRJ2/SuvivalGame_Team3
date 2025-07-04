using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public string itemDesc;
    public Sprite icon;
    public ItemData(string _itemName, string _itemDesc, Sprite _icon)
    {
        itemName = _itemName;
        itemDesc = _itemDesc;
        icon = _icon;
    }
}
