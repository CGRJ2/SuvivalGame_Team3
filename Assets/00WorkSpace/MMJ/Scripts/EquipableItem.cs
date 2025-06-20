using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipable Item", menuName = "Item/Equipable")]
public class EquipableItem : Item
{
    public EquipmentType equipmentType;

    protected override void Effect()
    {
        Debug.Log(itemName + " ภๅย๘ตส");
    }


}
