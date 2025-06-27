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

    [Header("일반 결과 아이템")]
    public DismantleNormalResult[] normalResults;

    [Header("레어 결과 아이템")]
    public DismantleRareResult[] rareResults;

    [Header("금지 아이템 (해체 불가)")]
    public List<string> dismantleBanList;

    public Inventory inventory;

    public void Dismantle(Item targetItem)
    {
        if (dismantleBanList.Contains(targetItem.itemName))
        {
            Debug.Log($"{targetItem.itemName} 은 해체 불가!");
            return;
        }

        // 아이템 1개 감소
        inventory.RemoveItem(targetItem, 1);

        // 일반 아이템 지급 (랜덤 수량)
        foreach (var result in normalResults)
        {
            int count = Random.Range(result.minCount, result.maxCount + 1);
            inventory.AcquireItem(result.item, count);
        }

        // 레어 아이템 확률 지급
        foreach (var rare in rareResults)
        {
            if (Random.value <= rare.probability)
            {
                inventory.AcquireItem(rare.item, 1);
            }
        }

        Debug.Log($"{targetItem.itemName} 해체 완료!");
    }
}