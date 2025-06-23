using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "new Item/item")]
public class Item : ScriptableObject
{
    public string itemName;         //�������̸�
    public Sprite itemImage;        //�������̹���
    public GameObject itemPrefab;   //������ ������
    public ItemType itemType;       //������ Ÿ��

    public enum ItemType
    { 
        Equipment, // ���
        Used, // �Ҹ�ǰ
        Ingredient, // ���
        ETC // ��Ÿ
    }
}
