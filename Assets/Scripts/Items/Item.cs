using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public ItemType itemType;       //������ Ÿ��
    public string itemName;         //������ �̸�
    [TextArea(3,10)]
    public string description;         //������ ����
    public Sprite imageSprite;        //������ �̹���
    public GameObject instancePrefab;   //������ ������

    public int maxCount; // �� ĭ�� �ִ� ���� ���� ����

    // ���� �ʱ�ȭ ����
    protected virtual void OnEnable()
    {
        // �⺻ �������ν��Ͻ� ������ ����. Ư�� �������� ��� �������ν��Ͻ� �������� �ٸ��ɷ� �޾��ָ� ��
        instancePrefab = Resources.Load<GameObject>("ItemInstancePrefabs/ItemInstance_Noraml");

        imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/DefaultImage");
    }
    
    // ��� => �κ��丮 ��Ŭ�� ��ȣ�ۿ� �� ȣ��
    public void UseInInventory(SlotData slotData, int multieUseCount = 1)
    {
        // ��� ������
        if (this is IEquipable equipable)
        {
            equipable.EquipToQuickSlot();
        }
        // �Һ� ������ (�Һ� �������ε� ���� ������ �ֵ��� ���� ���������� ����)
        else if (this is Item_Consumable consumable)
        {
            // �Һ��� ���� �� ȿ�� ����
            ConsumableSorting(consumable);

            // ���� �����Ϳ��� ���� ���̱�
            consumable.Consume(slotData, multieUseCount);
        }
        // ������ �������̶��
        else if (this is Item_Recipe recipe)
        {
            recipe.UnlockThisRecipe();
        }
    }

    // ��ɽ��� => �÷��̾ ������ ���¿��� ������ �� ȣ��
    public void ActivateEffectOnAttack(SlotData slotData) // ���⼭ slotData�� ���� Ȱ��ȭ �Ǿ��ִ� �������� ���� ���� �����͸� �����Ѵ�
    {
        // ��� ������
        if (this is IEquipable equipable)
        {
            if (this is Item_Throwing throwing)
            {
                throwing.OnAttackEffect();
                throwing.Consume(slotData);
            }
            else
            {
                equipable.OnAttackEffect();
            }
        }
        // �Һ� ������ (�Һ� �������ε� ���� ������ �ֵ��� ���� ���������� ����)
        else if (this is Item_Consumable consumable)
        {
            // �Һ��� ���� �� ȿ�� ����
            ConsumableSorting(consumable);

            // ���� �����Ϳ��� ���� ���̱�
            consumable.Consume(slotData);
        }
    }

    // �Һ��� ���� �� �з��ؼ� �� ������ �´� ȿ�� ����
    private void ConsumableSorting(Item_Consumable consumable)
    {
        if (consumable is Item_HealingHP hp)
        {
            hp.Hp_HealingRandomPart();
        }
        else if (consumable is Item_BodyPart part)
        {
            part.Hp_Init();
        }
        else if (consumable is Item_ChargeBattery charge)
        {
            charge.Battery_Healing();
        }
        else if (consumable is Item_NewBattery battery)
        {
            battery.Battery_Init();
        }
        else
        {
            consumable.ConsumeEffectInvoke();
        }
    }


    // ������ �ν��Ͻ� ����
    public void SpawnItem(Transform transform, int count = 1)
    {
        ItemInstance instance = Instantiate(instancePrefab, transform.position, transform.rotation).GetComponent<ItemInstance>();
        instance.InitInstance(this, count);
    }
}

public enum ItemType
{
    Equipment, // ���
    Consumalbe, // �Ҹ�ǰ
    Ingredient, // ���
    Function, // ���
    Quest, //����Ʈ
    ETC, // ��Ÿ
    AllType
}

[System.Serializable]
public class ItemRequirement
{
    public Item item;
    public int count;
}


public interface IEquipable
{
    public void EquipToQuickSlot();

    public void OnAttackEffect();
}

public interface IConsumable
{
    public void Consume(SlotData slotData, int multieUseCount = 1);
}

