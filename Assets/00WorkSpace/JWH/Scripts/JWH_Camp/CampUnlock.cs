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
            // 조건 없는 아이템은 해금
            if (string.IsNullOrEmpty(unlock.requiredRoomId))//|| openedRoomIds.Contains(unlock.requiredRoomId)
            {
                Debug.Log($"해금됨: {unlock.item.itemName})");// 실제 해금
            }
            else
            {
                Debug.Log($"보류됨: {unlock.item.itemName})");
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
    public string requiredRoomId; // 여기 비어 있으면 방 조건 없음
}

