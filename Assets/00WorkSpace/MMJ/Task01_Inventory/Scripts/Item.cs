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

    public enum ItemType
    { 
        Equipment, // ���
        Used, // �Ҹ�ǰ
        Ingredient, // ���
        Function, // ���
        Quest, //����Ʈ
        ETC // ��Ÿ
    }

    public virtual void Use()
    {

    }

    public void SpawnItem(Transform transform)
    {
        ItemInstance instance = Instantiate(itemPrefab, transform.position, transform.rotation).GetComponent<ItemInstance>();
        instance.item = this;
    }
}
