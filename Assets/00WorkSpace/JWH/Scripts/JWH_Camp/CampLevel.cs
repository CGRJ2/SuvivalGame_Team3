using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampLevel : MonoBehaviour
{
    public int CurrentLevel { get; private set; } = 1;
    public int MaxLevel = 5;
    public List<LevelRequirement> requirements;

    public bool TryLevelUp()
    {

        Debug.Log($"[CampLevel] ������ �õ�: ���� ���� {CurrentLevel}");
        if (CurrentLevel >= MaxLevel) return false;

        LevelRequirement req = requirements[CurrentLevel - 1];
        Inventory inventory = FindObjectOfType<Inventory>();
        Slot[] slots = inventory.GetComponentsInChildren<Slot>();

        int totalCount = 0;
        foreach (Slot slot in slots)
        {
            if (slot.item != null && slot.item.itemName == req.itemName)
            {
                totalCount += slot.itemCount;
            }
        }

        Debug.Log($" �ʿ� ������: {req.itemName} x {req.itemCount}, ���� ����: {totalCount}");

        if (totalCount >= req.itemCount)
        {
            // ������ �Һ�
            int remaining = req.itemCount;
            foreach (Slot slot in slots)
            {
                if (slot.item != null && slot.item.itemName == req.itemName)
                {
                    int consume = Mathf.Min(slot.itemCount, remaining);
                    slot.SetSlotCount(-consume);
                    remaining -= consume;
                    if (remaining <= 0) break;
                    Debug.Log($" {slot.item.itemName} x {consume} �Һ��");
                }
            }

            CurrentLevel++;
            Debug.Log($"ķ�� ������ {CurrentLevel}�� ����");
            return true;
        }
        Debug.LogWarning($"������ {req.itemName} �������� ������ ����");
        return false;
    }
}

[System.Serializable]
public class LevelRequirement
{
    public string itemName;
    public int itemCount;
}
