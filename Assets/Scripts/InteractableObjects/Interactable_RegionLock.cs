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
        // Äü½½·Ô ¼Õ¿¡ ¿­¼è°¡ ÀÖ´Ù¸é
        
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();


        Debug.Log($"¿­¼è »ç¿ë(E)");
    }
}
