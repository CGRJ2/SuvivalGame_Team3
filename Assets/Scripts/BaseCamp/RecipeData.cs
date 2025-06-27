using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "New Recipe/Recipe")]
public class RecipeData : ScriptableObject
{
    [Header("반드시! 해당 레시피에 해당하는 [레시피 아이템]의 이름과 동일하게 작성해주세요.")]
    public string recipeName;
    public Sprite icon;

    [Header("결과물 설정")]
    public Item resultItem;
    public int resultItemCount = 1;

    [Header("재료 설정")]
    public List<ItemRequirement> requiredItems;


    [Header("언락 여부")]
    public bool isUnlocked;

}
