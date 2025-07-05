using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Upgrade : InteractableBase
{
    public override void Interact()
    {
        base.Interact();

        Debug.Log($"���׷��̵� UI Ȱ��ȭ");
        UIManager.Instance.upgradeGroup.OpenPanel_Base();

    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();

        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"ķ�� ���׷��̵�: (E)";
    }
}
