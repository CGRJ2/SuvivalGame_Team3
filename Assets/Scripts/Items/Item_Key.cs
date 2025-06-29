using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/퀘스트 아이템/열쇠")]
public class Item_Key : Item, IEquipable
{
    public void EquipToQuickSlot()
    {
        // 퀵슬롯 등록
        PlayerManager.Instance.instancePlayer.Status.onHandItem = this;
    }

    public void OnAttackEffect()
    {
        // 플레이어가 해당 자물쇠 오브젝트의 상호작용 영역 안에 존재한다면
        Debug.Log("스테이지 매니저에서 해당 스테이지 언락");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Quest;
        maxCount = 1;
        itemName = this.name;
        imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/Sprite_Quest");
    }
}
