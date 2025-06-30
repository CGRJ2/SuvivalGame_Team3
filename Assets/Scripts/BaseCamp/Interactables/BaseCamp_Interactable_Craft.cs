using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Craft : InteractableBase
{
    public override void Interact()
    {
        Debug.Log($"제작 UI 활성화");
        UIManager.Instance.craftingUI.craftingUIGroup.PanelOpen();
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : 상호작용 범위 진입");
    }
}
