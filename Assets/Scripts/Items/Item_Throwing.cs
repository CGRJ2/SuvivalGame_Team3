using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/¼Òºñ ¾ÆÀÌÅÛ/ÅõÃ´ ¾ÆÀÌÅÛ")]

public class Item_Throwing : Item_Consumable, IEquipable
{
    public void ActivateEffectOnAttack()
    {

    }


    public void EquipToQuickSlot()
    {
        // Äü½½·Ô ºóÄ­ ÆÇ´Ü & ºó Äü½½·Ô¿¡ ÀåÂø
    }

    public void OnAttackEffect()
    {
        throw new System.NotImplementedException();
    }
}
