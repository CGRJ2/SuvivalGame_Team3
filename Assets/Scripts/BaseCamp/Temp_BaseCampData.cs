using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_BaseCampData
{
    public ObservableProperty<int> CurrentCampLevel { get; private set; }
    // 조합 가능 아이템 정보 (조합 결과물 아이템, bool isUnlocked, 언락 트리거 아이템(레시피) => 이 아이템을 습득해서 사용()하면 언락해줌..)

    public Item_Recipe[] allRecipeList;
    public List<Item_Recipe> UnlockedRecipeList;

    public void Init()
    {
        allRecipeList = Resources.LoadAll<Item_Recipe>("ItemDatabase/99 Recipes");
    }

    public void LevelUp()
    {
        if (CurrentCampLevel.Value < BaseCampManager.Instance.MaxLevel)
            CurrentCampLevel.Value += 1;
        else Debug.Log("레벨업 불가 [사유 : 이미 최대 레벨입니다.]");
    }
    

    // 언락된 레시피들만 오픈
    public List<Item_Recipe> GetUnlockRecipeList()
    {
        List<Item_Recipe> unlockedRecipes = new List<Item_Recipe>();

        foreach (Item_Recipe recipe in allRecipeList)
        {
            // 레시피 리스트 안에서 (레시피 이름 == 사용한 레시피 아이템 이름) 인 경우 레시피 언락
            if (recipe.RecipeData.isUnlocked)
            {
                unlockedRecipes.Add(recipe);
            }
        }

        // 인덱스 순으로 재정렬
        unlockedRecipes.Sort((a, b) => a.RecipeData.orderIndex.CompareTo(b.RecipeData.orderIndex));

        return unlockedRecipes;
    }
}
