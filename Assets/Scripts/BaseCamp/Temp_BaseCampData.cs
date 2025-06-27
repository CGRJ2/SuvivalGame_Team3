using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_BaseCampData
{
    public ObservableProperty<int> CurrentCampLevel { get; private set; }
    // 조합 가능 아이템 정보 (조합 결과물 아이템, bool isUnlocked, 언락 트리거 아이템(레시피) => 이 아이템을 습득해서 사용()하면 언락해줌..)

    List<RecipeData> recipeList;

    public void LevelUp()
    {
        if (CurrentCampLevel.Value < BaseCampManager.Instance.MaxLevel)
            CurrentCampLevel.Value += 1;
        else Debug.Log("레벨업 불가 [사유 : 이미 최대 레벨입니다.]");
    }
    

    // 레시피 아이템 사용 효과를 적용하려면 key값 받아와서 구분 후 언락
    public void UnlockRecipe(string recipeItemName)
    {

        foreach (RecipeData recipe in recipeList)
        {
            // 레시피 리스트 안에서 (레시피 이름 == 사용한 레시피 아이템 이름) 인 경우 레시피 언락
            if (recipe.recipeName == recipeItemName)
            {
                // 이미 언락되어 있는 상태라면
                if (recipe.isUnlocked)
                {
                    Debug.Log("이미 알고 있는 조합법이다. ");
                }
                else
                {
                    recipe.isUnlocked = true;
                }
            }
        }
    }
}
