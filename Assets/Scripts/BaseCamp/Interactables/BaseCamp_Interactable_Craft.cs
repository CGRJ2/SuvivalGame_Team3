using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Craft : InteractableBase
{
    public override void Interact()
    {
        Debug.Log($"���� UI Ȱ��ȭ");
        UIManager.Instance.craftingUI.craftingUIGroup.PanelOpen();
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ���� ����");
    }
}
