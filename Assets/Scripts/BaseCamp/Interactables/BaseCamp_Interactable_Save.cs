using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Save : InteractableBase
{
    //  ��ȣ�ۿ��� UI�� ���� �뵵�� ��, �Ʒ� ������ ���� UI���� �Ǵ�/�����ϵ��� �и��Ѵ�
    public override void Interact()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ����");
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : ��ȣ�ۿ� ���� ����");
    }
}
