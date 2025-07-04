using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Craft : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log($"���� UI Ȱ��ȭ");
        UIManager.Instance.craftingGroup.OpenPanel_CraftRecipeList();
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();
        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"���۴� ����: (E)";
    }

}
