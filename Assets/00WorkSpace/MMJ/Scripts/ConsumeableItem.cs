using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumeable Item", menuName = "Item/Consumeable")]
public class ConsumeableItem : Item
{
    public int 체력_혹은_정신력_수치;

    protected override void Effect()
    {
        Debug.Log(itemName + " 사용됨."); 
        Debug.Log(체력_혹은_정신력_수치 + " 만큼 회복됨.");
    }
}
