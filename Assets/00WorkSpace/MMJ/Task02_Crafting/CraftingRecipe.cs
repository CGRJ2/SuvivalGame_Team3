using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;       //레시피이름
    public Sprite icon;             //레시피 이미지


    public Item[] requiredItems;    //필요한아이템
    public int[] requiredCounts;    //필요아이템수량


    public Item resultItem;         //결과아이템
    public int resultCount = 1;         //결과아이템수량

    public float craftDuration = 3f; // 제작에 걸리는 시간 (초 단위)
}
