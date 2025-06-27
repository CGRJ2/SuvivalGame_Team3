using System.Collections.Generic;
using UnityEngine;

public class DismantleManager : MonoBehaviour
{
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

    public Inventory inventory;

    public void Dismantle(Item targetItem)
    {
        if (dismantleBanList.Contains(targetItem.itemName))
        {
            Debug.Log($"{targetItem.itemName} �� ��ü �Ұ�!");
            return;
        }

        // ������ 1�� ����
        inventory.RemoveItem(targetItem, 1);

        // �Ϲ� ������ ���� (���� ����)
        foreach (var result in normalResults)
        {
            int count = Random.Range(result.minCount, result.maxCount + 1);
            inventory.AcquireItem(result.item, count);
        }

        // ���� ������ Ȯ�� ����
        foreach (var rare in rareResults)
        {
            if (Random.value <= rare.probability)
            {
                inventory.AcquireItem(rare.item, 1);
            }
        }

        Debug.Log($"{targetItem.itemName} ��ü �Ϸ�!");
    }
}