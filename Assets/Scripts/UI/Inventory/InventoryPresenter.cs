using System;
using UnityEngine;


[System.Serializable]
public class InventoryPresenter : IDisposable
{
    public InventoryModel model; // ==> DataField

    public InventoryView view; // ==> UI


    // �⺻ ������ => ó�� ����
    public InventoryPresenter()
    {
        this.model = new InventoryModel();
        // ������ �ε��� �� Status�� �ε��� �����ͷ� ��ü
        
    }


    // ����ȯ �� �κ��丮 ĵ������ �޶��� �� ����. UIManager�� inventoryView ���� ����
    public void SetView(InventoryView view)
    {
        this.view = view;
        this.view.CurrentTab.Subscribe(UpdateSlotsToCurrentTab);
    }

    public void AddItem(Item item, int count = 1)
    {
        // �߰� �������� ���� �Ǵ�

        if (model.CanAddItem(item, count))
        {
            model.AddItem(item, count);
            // ui �ݿ�
            UpdateUI();

            // ������ ȹ�� UI �˾� �޼��� ����
            UIManager.Instance.popUpUIGroup.CollectMessageUI.PopMessage($"������ ȹ�� : {item.itemName} x{count}");
        }
        else
        {
            // �κ��丮 �ڸ� ���� UI �˾� ����
            // UIManager.[�κ��丮 �ڸ��� �����մϴ�] Ȱ��ȭ
        }
    }

    public void RemoveItem(Item item, int count = 1)
    {
        model.RemoveItem(item, count);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (view == null) SetView(UIManager.Instance.inventoryGroup.inventoryView);
        // 1. �κ��丮�� ������ ��
        // 2. �κ��丮 ���ο��� �巡�� �� ����� �߻����� ��
        // 3. �κ��丮�� Ȱ��ȭ �Ǿ��ִ� ���¿��� �������� �߰�/���ŵǾ��� ��
        UpdateSlotsToCurrentTab(view.CurrentTab.Value);
    }

    public void UpdateSlotsToCurrentTab(ItemType tabType)
    {
        view.UpdateInventorySlotView(model.GetCurrentTabSlots(tabType));
    }

    public void Dispose()
    {
        this.view.CurrentTab.Unsubscribe(UpdateSlotsToCurrentTab);

    }
}
