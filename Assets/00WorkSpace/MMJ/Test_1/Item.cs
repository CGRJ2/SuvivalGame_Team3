using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "new Item/item")]
public class Item : ScriptableObject
{
    public string itemName;         //아이템이름
    public Sprite itemImage;        //아이템이미지
    public GameObject itemPrefab;   //아이템 프리펩
    public ItemType itemType;       //아이템 타입

    public enum ItemType
    { 
        Equipment, // 장비
        Used, // 소모품
        Ingredient, // 재료
        ETC // 기타
    }
}
