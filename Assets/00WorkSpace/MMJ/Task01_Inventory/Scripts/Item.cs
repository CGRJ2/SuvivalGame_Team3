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

    // �Ϲ� ���
    public void Use()
    {
        Debug.Log($"[{itemName}] �����");
    }

    // �Һ�
    public void Use(SlotData slotData, int multieUseCount = 1)
    {
        Debug.Log($"[{itemName}] �Һ�");

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
    Equipment, // ���
    Used, // �Ҹ�ǰ
    Ingredient, // ���
    Function, // ���
    Quest, //����Ʈ
    ETC // ��Ÿ
}
