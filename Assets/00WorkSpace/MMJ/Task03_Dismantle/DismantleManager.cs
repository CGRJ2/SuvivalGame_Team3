using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DismantleManager : MonoBehaviour
{
    public static bool dismantleActivated = false;
    [SerializeField] private GameObject go_dismantleBase;
    [System.Serializable]
    public class DismantleNormalResult
    {
        public Item item;
        public int minCount;
        public int maxCount;
    }

    [System.Serializable]
    public class DismantleRareResult
    {
        public Item item;
        [Range(0f, 1f)] public float probability;
    }

    [Header("�Ϲ� ��� ������")]
    public DismantleNormalResult[] normalResults;

    [Header("���� ��� ������")]
    public DismantleRareResult[] rareResults;

    [Header("���� ������ (��ü �Ұ�)")]
    public List<string> dismantleBanList;

    [Header("UI ����")]
    public Slot[] dismantleSlots;
    public Button dismantleButton;

    public Inventory inventory;

    private void Start()
    {
        if (dismantleButton != null)
            dismantleButton.onClick.AddListener(OnClickDismantle);
    }
    private void Update()
    {
        TryOpenDismantle();
    }

    void TryOpenDismantle()
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

    private void OnClickDismantle()
    {
        foreach (var slot in dismantleSlots)
        {
            if (slot.item != null && slot.itemCount > 0)
            {
                int count = slot.itemCount;

                for (int i = 0; i < count; i++)
                {
                    if (dismantleBanList.Contains(slot.item.itemName))
                    {
                        Debug.Log($"{slot.item.itemName} �� ��ü �Ұ�!");
                        continue;
                    }

                    inventory.RemoveItem(slot.item, 1);
                    GiveRewards();
                }

                slot.ClearSlot();
            }
        }
    }

    private void GiveRewards()
    {
        foreach (var normal in normalResults)
        {
            int count = Random.Range(normal.minCount, normal.maxCount + 1);
            inventory.AcquireItem(normal.item, count);
        }

        foreach (var rare in rareResults)
        {
            if (Random.value <= rare.probability)
            {
                inventory.AcquireItem(rare.item, 1);
            }
        }
    }
}