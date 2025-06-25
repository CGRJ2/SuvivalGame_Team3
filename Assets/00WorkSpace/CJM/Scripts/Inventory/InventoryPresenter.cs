using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter
{
    public InventoryModel model = new InventoryModel(); // ==> DataField

    public InventoryView view; // ==> UI


    // �⺻ ������ => ó�� ����
    public InventoryPresenter()
    {
        this.model = new InventoryModel();
        this.view = UIManager.Instance.InventoryPanel.GetComponent<InventoryView>();
    }

    // �ε� ��
    public InventoryPresenter(InventoryModel model)
    {
        this.model = model;
        this.view = UIManager.Instance.InventoryPanel.GetComponent<InventoryView>();
    }

    public void AddItem(Item item, int count = 1)
    {
        // �߰� �������� ���� �Ǵ�
        if (model.CanAddItem(item, count))
        {
            model.AddItem(item, count);
        }
        else
        {
            // �κ��丮 �ڸ� ���� UI �˾� ����
            // UIManager.[�κ��丮 �ڸ��� �����մϴ�] Ȱ��ȭ
        }
    }
    public void UpdateUI()
    {
        // 1. �κ��丮�� ������ ��
        // 2. �κ��丮 ���ο��� �巡�� �� ����� �߻����� ��
        // 3. �κ��丮�� Ȱ��ȭ �Ǿ��ִ� ���¿��� �������� �߰�/���ŵǾ��� ��
        // view.���ñ⹹�ñ�
    }
}
