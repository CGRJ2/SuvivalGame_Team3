using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeID;         //레시피ID
    public Item[] requiredItems;    //필요한아이템
    public int[] requiredCounts;    //필요아이템수량
    public Item outputItem;         //결과아이템
    public int outputCount = 1;     //결과아이템수량
}
