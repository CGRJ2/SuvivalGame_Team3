using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Save : InteractableBase
{
    public override void Interact()
    {
        base.Interact();

        PlayerManager.Instance.SaveInBaseCamp();
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();
        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"저장하고 다음날로: (E)";
    }
}
