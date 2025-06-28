using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/퀘스트 아이템/일반")]
public class Item_Quest : Item
{
    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Quest;
        maxCount = 1;
        itemName = this.name;
        imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/Sprite_Quest");
    }
}
