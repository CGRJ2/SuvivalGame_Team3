using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Upgrade : InteractableBase
{
    public override void Interact()
    {
        Debug.Log($"���׷��̵� UI Ȱ��ȭ");
        UIManager.Instance.upgradeGroup.OpenPanel_Base();

    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ���� ����");
    }
}
