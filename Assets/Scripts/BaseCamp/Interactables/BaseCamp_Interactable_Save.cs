using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Save : InteractableBase
{
    public override void Interact()
    {
        //UIManager.Instance.SaveUI
        Debug.Log($"Save UI Ȱ��ȭ");
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ���� ����");
    }
}
