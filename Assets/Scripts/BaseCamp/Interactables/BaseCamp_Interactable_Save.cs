using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Save : InteractableBase
{
    public override void Interact()
    {
        //UIManager.Instance.SaveUI
        Debug.Log($"Save UI 활성화");
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : 상호작용 범위 진입");
    }
}
