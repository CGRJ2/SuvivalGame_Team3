using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Save : InteractableBase
{
    public override void Interact()
    {
        base.Interact();

        //UIManager.Instance.SaveUI
        Debug.Log($"Save UI Ȱ��ȭ");
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();
        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"�Ϸ� ������: (E)";
    }
}
