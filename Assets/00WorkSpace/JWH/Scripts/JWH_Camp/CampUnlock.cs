using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampUnlock : MonoBehaviour
{
    public List<UnlockData> unlockByLevel;

    public void UnlockItems(int level)
    {
        foreach (var data in unlockByLevel)
        {
            if (data.level == level)
            {
                foreach (var item in data.itemsToUnlock)
                {
                    Debug.Log($"{item.itemName} 아이템이 해제되었습니다.");
                    //여따가 기능추가
                    
                }
                return;
            }
        }

       
    }
}

[System.Serializable]
public class UnlockData
{
    public int level;
    public List<Item> itemsToUnlock;
}

