using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class Interactable_RegionLock : InteractableBase
{

    public Item itemForUnlock;

    public override void Interact()
    {
        base.Interact();
        // ������ �տ� ���谡 �ִٸ�
        
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();


        Debug.Log($"���� ���(E)");
    }
}
