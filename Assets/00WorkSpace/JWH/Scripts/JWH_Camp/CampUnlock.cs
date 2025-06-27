using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampUnlock : MonoBehaviour
{
    //public CampRoomChecker roomChecker;
    public List<UnlockData> unlockByLevel;

    public void UnlockItems(int level)// List<string> openedRoomIds
    {

        var data = unlockByLevel.Find(d => d.level == level);
        if (data == null) return;
        foreach (var unlock in data.itemsToUnlock)
        {
            // ���� ���� �������� �ر�
            if (string.IsNullOrEmpty(unlock.requiredRoomId))//|| openedRoomIds.Contains(unlock.requiredRoomId)
            {
                Debug.Log($"�رݵ�: {unlock.item.itemName})");// ���� �ر�
            }
            else
            {
                Debug.Log($"������: {unlock.item.itemName})");
            }
        }
    }
}

[System.Serializable]
public class UnlockData
{
    public int level;
    public List<RealUnlockItem> itemsToUnlock;
}


[System.Serializable]
public class RealUnlockItem
{
    public Item item;

    [HideInInspector]
    public string requiredRoomId; // ���� ��� ������ �� ���� ����
}

