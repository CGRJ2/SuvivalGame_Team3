using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Save : InteractableBase
{
    public override void Interact()
    {
        base.Interact();

        //UIManager.Instance.SaveUI
        Debug.Log($"Save UI 劝己拳");
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();
        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"窍风 付公府: (E)";
    }
}
