using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/장비 아이템/무기 아이템")]

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
        Debug.Log("아직 별 효과는 없음. 무기 공격력만 플레이어의 Attack에서 반영될거임");
    }
}

public enum WeaponAttackType
{
    Swing, Thrust, Default
}
