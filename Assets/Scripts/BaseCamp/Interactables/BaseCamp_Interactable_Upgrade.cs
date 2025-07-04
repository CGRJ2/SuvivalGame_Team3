using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Upgrade : InteractableBase
{
    public override void Interact()
    {
        base.Interact();

        Debug.Log($"업그레이드 UI 활성화");
        UIManager.Instance.upgradeGroup.OpenPanel_Base();

    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();

        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"캠프 업그레이드: (E)";
    }
}
