using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/�Һ� ������/�Ϲ� �Һ� ������")]
public class Item_Consumable : Item, IConsumable
{
    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Consumalbe;
        maxCount = 10;
        itemName = this.name;
        imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/Sprite_Consumable");
    }


    // Ŀ���� �Һ� ȿ���� �ִٸ� ����
    public void ConsumeEffectInvoke()
    {
        if (ItemDatabase.ConsumeEffectDic.ContainsKey(itemName)) 
            ItemDatabase.ConsumeEffectDic[itemName].Invoke();
    }

    // �� �������� �����ϴ� ���� ���� �����Ϳ� ������ ���� ����
    public void Consume(SlotData slotData, int multieUseCount = 1)
    {
        slotData.currentCount -= multieUseCount;
        if (slotData.currentCount <= 0)
        {
            slotData.CleanSlotData();
        }
    }
}
