using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Interactable_Farming : InteractableBase
{
    enum FarmingType
    {
        Drop_Immediately, Drop_AfterAnimation, AddToInventory_Immediately, AddToInventory_AfterAnimation
    }
    [SerializeField] Item dropItem;
    [SerializeField] FarmingType dropType;

   
    public override void Interact()
    {
        base.Interact();

        switch (dropType)
        {
            case FarmingType.Drop_Immediately:

                // ������ �ν��Ͻ� ���
                dropItem.SpawnItem(transform);

                break;
            case FarmingType.Drop_AfterAnimation:

                // �ִϸ��̼� ���� �Ϸ� �� ����
                dropItem.SpawnItem(transform);

                break;
            case FarmingType.AddToInventory_Immediately:

                // �÷��̾� �κ��丮�� ��
                Debug.Log(pc);
                pc.Status.inventory.AddItem(dropItem);

                break;
            case FarmingType.AddToInventory_AfterAnimation:

                // �÷��̾� �κ��丮�� ��
                pc.Status.inventory.AddItem(dropItem);
                break;
        }
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : ä��(E) �˾� UI Ȱ��ȭ");
    }
}
