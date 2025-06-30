using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class Interactable_RegionLock : InteractableBase
{
    public Item itemForUnlock;
    public GameObject blockWall;

    public override void Interact()
    {
        base.Interact();

        // �տ� ��� �ִ� �������� �䱸 �����۰� ���� ���
        if (pc.Status.onHandItem == itemForUnlock)
        {
            Debug.Log("��� ����");
            gameObject.SetActive(false);
        }
        
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        // �տ� ��� �ִ� �������� �䱸 �����۰� ���� ��� => ��ȣ�ۿ� ����
        if (pc.Status.onHandItem == itemForUnlock)
            Debug.Log($"���� ���(E)");
    }
}
