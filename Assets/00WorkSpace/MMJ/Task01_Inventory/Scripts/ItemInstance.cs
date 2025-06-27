using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : InteractableBase
{
    public Item item;
    public int count;

    public void InitInstance(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public override void Interact()
    {
        base.Interact();

        // �÷��̾� �κ��丮�� ��
        pc.Status.inventory.AddItem(item, count);
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : ������ ����(E) �˾� UI Ȱ��ȭ");
    }
}
