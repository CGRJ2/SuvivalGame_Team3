using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/��� ������/���� ������")]

public class Item_Weapon : Item, IEquipable
{
    [field: SerializeField] public int Damage { get; private set; } = 1;
    public WeaponAttackType attackType;
    

    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Equipment;
        maxCount = 1;
        itemName = this.name;
    }

    /*public void EquipToQuickSlot()
    {
        PlayerManager.Instance.instancePlayer.Status.onHandItem = this;
    }*/

    public void OnAttackEffect()
    {
        Debug.Log("���� �� ȿ���� ����. ���� ���ݷ¸� �÷��̾��� Attack���� �ݿ��ɰ���");
    }
}

public enum WeaponAttackType
{
    Swing, Thrust, Default
}
