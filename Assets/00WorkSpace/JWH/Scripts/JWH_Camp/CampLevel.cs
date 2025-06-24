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
        if (CurrentLevel >= MaxLevel) return false;

        LevelRequirement req = requirements[CurrentLevel - 1];
        //if (PlayerInventory.Instance.HasItems(req.requiredItems))
        //{
        //    PlayerInventory.Instance.ConsumeItems(req.requiredItems);
        //    CurrentLevel++;
        //    return true;
        //}
        return false;
    }
}

[System.Serializable]
public class LevelRequirement
{
    //public List<InventoryItem> requiredItems;
}
