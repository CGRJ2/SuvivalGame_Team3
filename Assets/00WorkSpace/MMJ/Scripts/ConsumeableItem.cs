using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumeable Item", menuName = "Item/Consumeable")]
public class ConsumeableItem : Item
{
    public int ü��_Ȥ��_���ŷ�_��ġ;

    protected override void Effect()
    {
        Debug.Log(itemName + " ����."); 
        Debug.Log(ü��_Ȥ��_���ŷ�_��ġ + " ��ŭ ȸ����.");
    }
}
