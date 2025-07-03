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
    public Transform OriginTransform { get; set; }

    private void OnDisable()
    {
        DeactiveAction?.Invoke();
    }

    // ������̺� ������ ���Ե� ������ŭ ������ �ν��Ͻ� ���� �ݺ�
    public void DropItemInstances(DropInfo dropInfo)
    {
        for (int i = 0; i < dropInfo.dropCount; i++)
        {
            dropInfo.dropItem.SpawnItem(itemSpawnPoint);
        }
    }

    // �Ĺ� �Ϸ� �� ��Ȱ��ȭ or �ı�
    public void DeactiveAfterFarmingDone()
    {
        // �ϴ� �ı� ===> ������ƮǮ �������� �����ؾߵ�
        Destroy(gameObject);
    }

    public override void Interact()
    {
        base.Interact();
        DropInfo dropInfo = dropTable.GetDropItemInfo();

        switch (dropType)
        {
            case FarmingType.Drop_Immediately:
                // ������ŭ ������ �ν��Ͻ� ���
                DropItemInstances(dropInfo);
                DeactiveAfterFarmingDone();
                break;

            case FarmingType.Drop_AfterAnimation:
                // �ִϸ��̼� ���� ����.
                // �ִϸ��̼� ���� �Ϸ� �� ����
                // ==> �ش� �Ĺ� �ִϸ��̼��� ������ �������� �ִϸ��̼� �̺�Ʈ�� DropItemInstances(dropInfo)�� �����ϴ� �ɷ�
                DropItemInstances(dropInfo);
                DeactiveAfterFarmingDone();
                break;

            case FarmingType.AddToInventory_Immediately:
                // �÷��̾� �κ��丮�� ��
                pc.Status.inventory.AddItem(dropInfo.dropItem, dropInfo.dropCount);
                DeactiveAfterFarmingDone();
                break;

            case FarmingType.AddToInventory_AfterAnimation:
                // �ִϸ��̼� ���� ����.
                // �ִϸ��̼� ���� �Ϸ� �� ����
                // �÷��̾� �κ��丮�� ��
                pc.Status.inventory.AddItem(dropInfo.dropItem, dropInfo.dropCount);
                DeactiveAfterFarmingDone();
                break;
        }
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : ä��(E) �˾� UI Ȱ��ȭ");
    }
    
    
}
