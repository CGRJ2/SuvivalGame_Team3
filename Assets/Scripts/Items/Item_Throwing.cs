using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/�Һ� ������/��ô ������")]

public class Item_Throwing : Item_Consumable, IEquipable
{
    public void ActivateEffectOnAttack()
    {

    }


    public void EquipToQuickSlot()
    {
        // ������ ��ĭ �Ǵ� & �� �����Կ� ����
        PlayerManager.Instance.instancePlayer.Status.onHandItem = this;
    }

    public void OnAttackEffect()
    {
        throw new System.NotImplementedException();
    }
}
