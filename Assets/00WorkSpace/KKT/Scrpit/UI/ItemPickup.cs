using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "����"; // Inspector���� ������ �̸� ����
    public float pickupMessageTime = 3f; // �޽��� ǥ�� �ð�

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ������ ���� ���� (�κ��丮 �߰��Ǹ� ���)
            // InventorySystem.Instance.Add(itemName);  < �Ƹ���?

            // �˸� UI ���
            UIController.Instance.ShowCollectNotification($"{itemName}��(��) ȹ���߽��ϴ�!", pickupMessageTime);

            // ������ ������Ʈ ����(Ȥ�� ��Ȱ��ȭ)
            Destroy(gameObject);
        }
    }
}
