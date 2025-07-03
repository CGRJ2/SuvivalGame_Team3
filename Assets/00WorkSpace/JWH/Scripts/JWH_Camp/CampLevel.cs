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
        Debug.Log($"CampLevl ������ �õ�: ���� ���� {CurrentLevel}");

        if (CurrentLevel >= MaxLevel) return false;//�ִ뷹��

        LevelRequirement req = requirementsByLevel[CurrentLevel - 1];
        Inventory inventory = FindObjectOfType<Inventory>();//�κ����� ã��

        Slot[] slots = inventory.GetComponentsInChildren<Slot>();
        int totalCount = 0;
        foreach (Slot slot in slots)
        {
            if (slot.item != null && slot.item == req.Campitem)
            {
                totalCount += slot.itemCount;
            }
        }

        Debug.Log($" �ʿ� ������: {req.Campitem} x {req.CampitemCount}, ���� ����: {totalCount}");

        if (totalCount >= req.CampitemCount)
        {
            // ������ �Һ�
            int remaining = req.CampitemCount;
            foreach (Slot slot in slots)
            {
                if (slot.item != null && slot.item == req.Campitem)
                {
                    int consume = Mathf.Min(slot.itemCount, remaining);
                    slot.SetSlotCount(-consume);
                    remaining -= consume;
                    if (remaining <= 0) break;
                    Debug.Log($" {slot.item} x {consume} �Һ��");
                }
            }

            CurrentLevel++;
            Debug.Log($"ķ�� ������ {CurrentLevel}�� ����");
            return true;
        }
        Debug.LogWarning($"������ {req.Campitem} �������� ������ ����");
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