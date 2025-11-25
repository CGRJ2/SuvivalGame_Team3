using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Craft : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log($"제작 UI 활성화");
        Manager.ui.craftingGroup.OpenPanel_CraftRecipeList();
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();
        Manager.ui.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"제작대 열기: (E)";
    }

}
