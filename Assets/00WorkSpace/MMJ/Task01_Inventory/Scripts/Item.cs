using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "new Item/item")]
public class Item : ScriptableObject
{
    public string itemName;         //아이템 이름
    [TextArea]
    public string itemDesc;         //아이템 설명
    public Sprite itemImage;        //아이템 이미지
    public GameObject itemPrefab;   //아이템 프리펩
    public ItemType itemType;       //아이템 타입

    public int maxCount = 1; // 한 칸당 최대 보유 가능 수량


    // 소비 효과 실행
    public void AdjustConsumeEffect(SlotData slotData, int multieUseCount = 1)
    {
        // 슬롯 데이터에서 아이템 개수 감소
        slotData.currentCount -= multieUseCount;
        if (slotData.currentCount <= 0)
        {
            slotData.CleanSlotData();
        }

        // 효과 실행
        ItemDatabase.ConsumeEffectDic[itemName].Invoke();
    }

    // 장비 아아이템 장착 시 효과 적용
    public void AdjustEquipEffect()
    {
        ItemDatabase.EquipEffectDic[itemName].Invoke();
    }

    public bool IsCanEquip()
    {
        if (ItemDatabase.EquipEffectDic.ContainsKey(itemName)) { return true; }
        else return false;
    }

    public bool IsCanConsume()
    {
        if (ItemDatabase.ConsumeEffectDic.ContainsKey(itemName)) { return true; }
        else return false;
    }

    // 아이템 인스턴스 생성
    public void SpawnItem(Transform transform, int count = 1)
    {
        ItemInstance instance = Instantiate(itemPrefab, transform.position, transform.rotation).GetComponent<ItemInstance>();
        instance.InitInstance(this, count);
    }
}

public enum ItemType
{
    Equipment, // 장비
    Used, // 소모품
    Ingredient, // 재료
    Function, // 기능
    Quest, //퀘스트
    ETC // 기타
}

