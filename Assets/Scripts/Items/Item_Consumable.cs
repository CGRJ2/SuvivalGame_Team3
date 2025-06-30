using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/소비 아이템/일반 소비 아이템")]
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


    // 커스텀 소비 효과가 있다면 실행
    public void ConsumeEffectInvoke()
    {
        if (ItemDatabase.ConsumeEffectDic.ContainsKey(itemName)) 
            ItemDatabase.ConsumeEffectDic[itemName].Invoke();
    }

    // 이 아이템이 존재하는 원본 슬롯 데이터에 아이템 개수 감소
    public void Consume(SlotData slotData, int multieUseCount = 1)
    {
        slotData.currentCount -= multieUseCount;
        if (slotData.currentCount <= 0)
        {
            slotData.CleanSlotData();
        }
    }
}
