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
                //foreach (var item in data.itemsToUnlock)
                //{
                //    PlayerInventory.Instance.UnlockItem(item);
                //}
            }
        }
    }
}

[System.Serializable]
public class UnlockData
{
    public int level;
    //public List<ItemData> itemsToUnlock;
}

