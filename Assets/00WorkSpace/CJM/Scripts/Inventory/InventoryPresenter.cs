using System;
using UnityEngine;

public class InventoryPresenter 
{
    public InventoryModel model; // ==> DataField

    public InventoryView view; // ==> UI


    // �⺻ ������ => ó�� ����
    public InventoryPresenter()
    {
        this.model = new InventoryModel();
    }

    // �ε�� �Լ�
    public void SetModel(InventoryModel model)
    {
        this.model = model;
    }

    // ����ȯ �� �κ��丮 ĵ������ �޶��� �� ����. UIManager�� inventoryView ���� ����
    public void SetView(InventoryView view)
    {
        this.view = view;
        view.CurrentTab.Subscribe(UpdateSlotsToCurrentTab);
    }

    public void AddItem(Item item, int count = 1)
    {
        // �߰� �������� ���� �Ǵ�
        if (model.CanAddItem(item, count))
        {
            model.AddItem(item, count);
            // ui �ݿ�
            UpdateUI();
        }
        else
        {
            // �κ��丮 �ڸ� ���� UI �˾� ����
            // UIManager.[�κ��丮 �ڸ��� �����մϴ�] Ȱ��ȭ
        }
    }

    public void UpdateUI()
    {
        if (view == null) SetView(UIManager.Instance.inventoryUI.inventoryView);
        // 1. �κ��丮�� ������ ��
        // 2. �κ��丮 ���ο��� �巡�� �� ����� �߻����� ��
        // 3. �κ��丮�� Ȱ��ȭ �Ǿ��ִ� ���¿��� �������� �߰�/���ŵǾ��� ��
        UpdateSlotsToCurrentTab(view.CurrentTab.Value);
    }

    public void UpdateSlotsToCurrentTab(ItemType tabType)
    {
        view.UpdateInventorySlotView(model.GetCurrentTabSlots(tabType));
    }


}
