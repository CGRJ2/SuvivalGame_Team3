using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe : ScriptableObject
{
    public string recipeID;         //·¹½ÃÇÇID
    public Item[] requiredItems;    //
    public int[] requiredCounts;
    public Item outputItem;
    public int outputCount = 1;
}
