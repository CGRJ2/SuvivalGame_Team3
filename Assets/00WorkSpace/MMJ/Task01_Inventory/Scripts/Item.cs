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

    // 일반 사용
    public void Use()
    {
        Debug.Log($"[{itemName}] 사용함");
    }

    // 소비
    public void Use(SlotData slotData, int multieUseCount = 1)
    {
        Debug.Log($"[{itemName}] 소비");

        slotData.currentCount -= multieUseCount;
        if (slotData.currentCount <= 0)
        {
            slotData.item = null;
        }
    }


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
