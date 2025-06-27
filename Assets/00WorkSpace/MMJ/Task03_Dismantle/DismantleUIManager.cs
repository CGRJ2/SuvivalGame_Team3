using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DismantleUIManager : MonoBehaviour
{
    [Header("��ü ���� 10��")]
    public Slot[] dismantleSlots;

    [Header("�κ��丮 ����")]
    public Inventory inventory;

    [Header("��ü ��ư")]
    public Button dismantleButton;

    [Header("���� ������")]
    public Item cotton;
    public Item batteryPart;
    public Item dust;
    public Item plastic;
    public Item glue;
    public Item circuitBoard;

    [Header("���� ������ Ȯ��")]
    [Range(0f, 1f)]
    public float glueChance = 0.2f;
    [Range(0f, 1f)]
    public float circuitBoardChance = 0.1f;

    private void Start()
    {
        dismantleButton.onClick.AddListener(OnClickDismantle);
    }

    private void OnClickDismantle()
    {
        foreach (var slot in dismantleSlots)
        {
            if (slot.item != null && slot.itemCount > 0)
            {
                int stackCount = slot.itemCount;

                for (int i = 0; i < stackCount; i++)
                {
                    // 1. ������ 1���� �Ҹ�
                    inventory.RemoveItem(slot.item, 1);

                    // 2. �Ϲ� ������ ���� (1~5 ����)
                    inventory.AcquireItem(cotton, Random.Range(1, 6));
                    inventory.AcquireItem(batteryPart, Random.Range(1, 6));
                    inventory.AcquireItem(dust, Random.Range(1, 6));
                    inventory.AcquireItem(plastic, Random.Range(1, 6));

                    // 3. ���� ������ Ȯ��
                    if (Random.value < glueChance)
                        inventory.AcquireItem(glue, 1);
                    if (Random.value < circuitBoardChance)
                        inventory.AcquireItem(circuitBoard, 1);
                }

                // 4. ���� ����
                slot.ClearSlot();
            }
        }
    }
}