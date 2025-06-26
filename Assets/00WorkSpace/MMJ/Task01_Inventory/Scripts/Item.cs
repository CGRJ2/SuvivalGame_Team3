using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "new Item/item")]
public class Item : ScriptableObject
{
    public string itemName;         //������ �̸�
    [TextArea]
    public string itemDesc;         //������ ����
    public Sprite itemImage;        //������ �̹���
    public GameObject itemPrefab;   //������ ������
    public ItemType itemType;       //������ Ÿ��

    public int maxCount = 1; // �� ĭ�� �ִ� ���� ���� ����


    // �Һ� ȿ�� ����
    public void AdjustConsumeEffect(SlotData slotData, int multieUseCount = 1)
    {
        // ���� �����Ϳ��� ������ ���� ����
        slotData.currentCount -= multieUseCount;
        if (slotData.currentCount <= 0)
        {
            slotData.CleanSlotData();
        }

        // ȿ�� ����
        ItemDatabase.ConsumeEffectDic[itemName].Invoke();
    }

    // ��� �ƾ����� ���� �� ȿ�� ����
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

    // ������ �ν��Ͻ� ����
    public void SpawnItem(Transform transform, int count = 1)
    {
        ItemInstance instance = Instantiate(itemPrefab, transform.position, transform.rotation).GetComponent<ItemInstance>();
        instance.InitInstance(this, count);
    }
}

public enum ItemType
{
    Equipment, // ���
    Used, // �Ҹ�ǰ
    Ingredient, // ���
    Function, // ���
    Quest, //����Ʈ
    ETC // ��Ÿ
}

