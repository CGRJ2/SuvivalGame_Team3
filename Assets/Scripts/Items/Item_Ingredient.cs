using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/��� ������/�Ϲ� ���")]
public class Item_Ingredient : Item
{
    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Ingredient;
        maxCount = 30;
        itemName = this.name;
        imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/Sprite_Ingredient");
    }
}
