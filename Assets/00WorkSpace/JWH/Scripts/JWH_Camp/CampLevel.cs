/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampLevel : MonoBehaviour
{
    public int CurrentLevel { get; private set; } = 1;
    public int MaxLevel = 5;
    public List<LevelRequirement> requirementsByLevel;

    public bool TryLevelUp()
    {
        Debug.Log($"CampLevl 레벨업 시도: 현재 레벨 {CurrentLevel}");

        if (CurrentLevel >= MaxLevel) return false;//최대레벨

        LevelRequirement req = requirementsByLevel[CurrentLevel - 1];
        Inventory inventory = FindObjectOfType<Inventory>();//인벤에서 찾기

        Slot[] slots = inventory.GetComponentsInChildren<Slot>();
        int totalCount = 0;
        foreach (Slot slot in slots)
        {
            if (slot.item != null && slot.item == req.Campitem)
            {
                totalCount += slot.itemCount;
            }
        }

        Debug.Log($" 필요 아이템: {req.Campitem} x {req.CampitemCount}, 보유 수량: {totalCount}");

        if (totalCount >= req.CampitemCount)
        {
            // 아이템 소비
            int remaining = req.CampitemCount;
            foreach (Slot slot in slots)
            {
                if (slot.item != null && slot.item == req.Campitem)
                {
                    int consume = Mathf.Min(slot.itemCount, remaining);
                    slot.SetSlotCount(-consume);
                    remaining -= consume;
                    if (remaining <= 0) break;
                    Debug.Log($" {slot.item} x {consume} 소비됨");
                }
            }

            CurrentLevel++;
            Debug.Log($"캠프 레벨이 {CurrentLevel}로 증가");
            return true;
        }
        Debug.LogWarning($"아이템 {req.Campitem} 부족으로 레벨업 실패");
        return false;
    }
}


[System.Serializable]
 public class LevelRequirement
{
    public int Camplevel;
    public Item Campitem;
    public int CampitemCount;
}
*/