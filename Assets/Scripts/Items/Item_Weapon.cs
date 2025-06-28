using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/��� ������/���� ������")]

public class Item_Weapon : Item, IEquipable
{
    [field: SerializeField] public int Damage { get; private set; } = 1;

    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Equipment;
        maxCount = 1;
        itemName = this.name;
        imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/Sprite_Weapon");
    }

    public void EquipToQuickSlot()
    {
        throw new System.NotImplementedException();
    }

    public void OnAttackEffect()
    {
        Debug.Log("���� �� ȿ���� ����. ���� ���ݷ¸� �÷��̾��� Attack���� �ݿ��ɰ���");
    }
}
