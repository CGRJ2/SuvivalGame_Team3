using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : InteractableBase
{
    public Item item;

    public override void Interact()
    {
        base.Interact();

        // �÷��̾� �κ��丮�� ��
        pc.Status.inventory.AcquireItem(item);
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : ������ ����(E) �˾� UI Ȱ��ȭ");
    }
}
