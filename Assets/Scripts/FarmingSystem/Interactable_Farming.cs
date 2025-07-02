using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Farming : InteractableBase, ISpawnable
{
    enum FarmingType
    {
        Drop_Immediately, Drop_AfterAnimation, AddToInventory_Immediately, AddToInventory_AfterAnimation
    }
    [SerializeField] DropTable dropTable;
    [SerializeField] Transform itemSpawnPoint;
    [SerializeField] FarmingType dropType;

    public Action DeactiveAction { get; set; }

    private void OnDisable()
    {
        DeactiveAction?.Invoke();
    }

    public override void Interact()
    {
        base.Interact();
        DropInfo dropInfo = dropTable.GetDropItemInfo();

        switch (dropType)
        {
            case FarmingType.Drop_Immediately:

                // ������ �ν��Ͻ� ���
                dropInfo.dropItem.SpawnItem(itemSpawnPoint, dropInfo.dropCount);

                break;
            case FarmingType.Drop_AfterAnimation:

                // �ִϸ��̼� ���� �Ϸ� �� ����
                dropInfo.dropItem.SpawnItem(itemSpawnPoint, dropInfo.dropCount);

                break;
            case FarmingType.AddToInventory_Immediately:

                // �÷��̾� �κ��丮�� ��
                Debug.Log(pc);
                pc.Status.inventory.AddItem(dropInfo.dropItem, dropInfo.dropCount);

                break;
            case FarmingType.AddToInventory_AfterAnimation:

                // �÷��̾� �κ��丮�� ��
                pc.Status.inventory.AddItem(dropInfo.dropItem, dropInfo.dropCount);
                break;
        }
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : ä��(E) �˾� UI Ȱ��ȭ");
    }
}
