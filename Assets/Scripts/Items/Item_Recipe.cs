using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/재료 아이템/레시피")]
public class Item_Recipe : Item_Ingredient
{
    public string recipeName;
    public Sprite icon;

    [field: Header("레시피 데이터")]
    [field: SerializeField] public RecipeData RecipeData { get; private set; }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        maxCount = 1;
    }

    
    public void UnlockThisRecipe()
    {
        if (!RecipeData.isUnlocked)
            RecipeData.isUnlocked = true;
        else
        {
            Debug.Log("이미 제작대에 추가된 레시피이다.");
        }
    }
}

[System.Serializable]
public class RecipeData
{
    [Header("언락 여부")]
    public bool isUnlocked;

    [Header("레시피 창에 나타낼 순서")]
    public int orderIndex;

    [Header("제작 시간")]
    public float craftDuration = 3f;

    [Header("결과물 설정")]
    public Item resultItem;
    public int resultItemCount = 1;

    [Header("재료 설정")]
    public List<ItemRequirement> requiredItems;
}
