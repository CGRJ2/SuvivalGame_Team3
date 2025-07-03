using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/기능 아이템")]
public class Item_Funcion : Item
{
    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Function;
        maxCount = 1;
        itemName = this.name;
    }
}
