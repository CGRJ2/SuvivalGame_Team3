using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Upgrade : InteractableBase
{
    // Start is called before the first frame update
    public override void Interact()
    {
        Debug.Log($"���׷��̵� UI Ȱ��ȭ");
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ���� ����");
    }
}
