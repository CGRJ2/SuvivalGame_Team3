using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ��ü �ý����� �����ϴ� ��ũ��Ʈ�Դϴ�.
/// �÷��̾ �κ��丮 �������� ��ü�ϰ�, �׿� ���� ������ ȹ���ϴ� ����� ����մϴ�.
/// </summary>

public class DismantleManager : MonoBehaviour
{
    public static bool dismantleActivated = false;          // ��ü �ý��� UI Ȱ��ȭ ���¸� ��Ÿ���� ���� �����Դϴ�.
    [SerializeField] private GameObject go_dismantleBase;   // ��ü �ý����� �⺻ UI ������Ʈ, �� ������Ʈ�� Ȱ��ȭ, ��Ȱ��ȭ�� �Ѱ� ���ϴ�.
    [System.Serializable]
    public class DismantleNormalResult
    {
        public Item item;       // ��ü �� ȹ���� [�Ϲ�] ������
        public int minCount;    // ȹ���� �������� �ּ� ����
        public int maxCount;    // ȹ���� �������� �ִ� ����
    }

    [System.Serializable]
    public class DismantleRareResult
    {
        public Item item;                           // ��ü �� ȹ���� [����] ������
        [Range(0f, 1f)] public float probability;   // ������ ȹ�� Ȯ��
    }

    [Header("�Ϲ� ��� ������")]
    public DismantleNormalResult[] normalResults;   // ��ü �� �׻� ȹ���� �Ϲ� ������ ��� ���

    [Header("���� ��� ������")]
    public DismantleRareResult[] rareResults;       // ��ü �� Ȯ�������� ȹ���� ��� ������ ��� ���

    [Header("���� ������ (��ü �Ұ�)")]
    public List<string> dismantleBanList;           // ��ü�� �� ���� �����۵��� �̸� ��� (�ۼ� ���ô� MMJ_Test_Scene -> Canvas -> Dismantle �ν����Ϳ��� ���� Ȯ��)

    [Header("UI ����")]
    public Slot[] dismantleSlots;                   // �κ��丮���� ��� ���� ��Ȱ�� (�� ���Կ��� ALL�̶�� ���� Ÿ���� �߰��� ���� ����), ���Կ� �ִ� �����۵��� ��ü ����� ��
    public Button dismantleButton;                  // ����Ƽ UI ��ư, ��ư�� Ŭ���ϸ� ��ü ������ ����

    public Inventory inventory;                     // �κ��丮

    private void Start()
    {
        if (dismantleButton != null)
            dismantleButton.onClick.AddListener(OnClickDismantle); // ��ü ��ư�� Inspector�� ���� �Ҵ�Ǿ��ٸ�, Ŭ�� �� OnClickDismantle �޼��带 ȣ���ϵ��� �����ʸ� �߰��մϴ�.
    }
    private void Update()
    {
        TryOpenDismantle();     // �������� ���� ��ü UI�� ���� �ݴ� �Է��� ����
    }

    void TryOpenDismantle()     // Ư�� Ű �Է�(KeyCode.O)�� �����Ͽ� ��ü UI�� ���
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            dismantleActivated = !dismantleActivated;

            if (dismantleActivated)
                OpenDismantle();
            else
                CloseDismantle();
        }
    }

    void OpenDismantle()
    {
        go_dismantleBase.SetActive(true);
    }

    void CloseDismantle()
    {
        go_dismantleBase?.SetActive(false);
    }

    private void OnClickDismantle()                         // ��ü ��ư Ŭ�� �� ȣ��Ǵ� �޼���, ��ü ���Կ� �ִ� �����۵��� ó���ϰ� ������ ����
    {
        foreach (var slot in dismantleSlots)                // ��� ��ü ������ ��ȸ
        {
            if (slot.item != null && slot.itemCount > 0)    // ���Կ� �������� �ְ�, ������ ������ 0���� ���� ��쿡�� ó��
            {
                int count = slot.itemCount;                 // ���� ���Կ� �ִ� �������� ������ ������

                for (int i = 0; i < count; i++)             // ���Կ� �ִ� ������ ������ŭ �ݺ��Ͽ� �� �������� ���������� ��ü �õ�
                {
                    if (dismantleBanList.Contains(slot.item.itemName))      // ���� �������� �̸��� ��ü ���� ��Ͽ� �ִ��� Ȯ��
                    {
                        Debug.Log($"{slot.item.itemName} �� ��ü �Ұ�!");
                        continue;
                    }

                    inventory.RemoveItem(slot.item, 1);     // �κ��丮���� �ش� ������ 1���� ����
                    GiveRewards();                          // ������ ��ü�� ���� ������ ����
                }

                slot.ClearSlot();                           // ��� ������ ó�� �� �ش� ��ü ������ ���
            }
        }
    }

    private void GiveRewards()                              // ��ü�� �����ۿ� ���� ������ �κ��丮�� �߰��ϴ� �޼���
    {
        foreach (var normal in normalResults)               // ������ ��� �Ϲ� ���� �����ۿ� ���� ó��
        {
            int count = Random.Range(normal.minCount, normal.maxCount + 1); // �������� �ּ�-�ִ� ���� ���� ������ �������� ������ ����
            inventory.AcquireItem(normal.item, count);                      // �κ��丮�� ������ ������ŭ�� �������� �߰�
        }

        foreach (var rare in rareResults)                   // ������ ��� ��� ���� �����ۿ� ���� ó��
        {
            if (Random.value <= rare.probability)           // Ȯ���� ���� �������� ȹ���� ���
            {
                inventory.AcquireItem(rare.item, 1);        // �κ��丮�� �ش� ��� ������ 1���� �߰�
            }
        }
    }
}