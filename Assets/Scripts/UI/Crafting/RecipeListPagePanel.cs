using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeListPagePanel : MonoBehaviour
{
    [SerializeField] Transform recipeListParent;
    private RecipeSlotView[] recipeSlotViews;

    List<Item_Recipe> nowPageRecipeList = new List<Item_Recipe>();
    public int nowPageIndex;
    public int maxPageIndex;


    public void Init()
    {
        // 레시피 슬롯 리스트 초기화
        recipeSlotViews = recipeListParent.GetComponentsInChildren<RecipeSlotView>();
        // 현재 페이지 인덱스에 있는 레시피 리스트(처음엔 15개 전부 null)
        for (int i = 0; i < 15; i++)
        {
            nowPageRecipeList.Add(null);
        }
    }

    public void SetCurrentPageRecipeList(int start, int end)
    {
        Item_Recipe[] allRecipeList = BaseCampManager.Instance.allRecipeList;

        for (int i = start; i < end; i++)
        {
            if (allRecipeList.Length <= i)
            {
                Debug.Log(i - nowPageIndex * recipeSlotViews.Length);
                nowPageRecipeList[i - nowPageIndex * recipeSlotViews.Length] = null;
            }
            else
            {
                nowPageRecipeList[i - nowPageIndex * recipeSlotViews.Length] = allRecipeList[i];
            }
        }
    }

    // 현재 페이지 인덱스에 맞는 레시피 아이템 => 레시피
    public void UpdateRecipePageData()
    {
        // 임시
        if (nowPageIndex == 0)
        {
            SetCurrentPageRecipeList(0, 15);
        }
        else if (nowPageIndex == 1)
        {
            SetCurrentPageRecipeList(15, 30);
        }
        else if (nowPageIndex == 2)
        {
            SetCurrentPageRecipeList(30, 45);
        }
        else if (nowPageIndex == 3)
        {
            SetCurrentPageRecipeList(45, 60);
        }

        for (int i = 0; i < recipeSlotViews.Length; i++)
        {
            if (nowPageRecipeList[i] != null)
            {
                recipeSlotViews[i].gameObject.SetActive(true);

                // 열려있는 레시피라면 => UI에 아이템 스프라이트 넣기
                if (nowPageRecipeList[i].RecipeData.isUnlocked)
                {
                    recipeSlotViews[i].recipeItem = nowPageRecipeList[i];
                    recipeSlotViews[i].SetRecipeSprite();
                }
                // 잠겨있는 레시피라면 => UI에 검은 창으로 덮기
                else
                {
                    recipeSlotViews[i].recipeItem = null;
                    recipeSlotViews[i].SetBlockedSprite();
                }
            }
            else
            {
                recipeSlotViews[i].gameObject.SetActive(false);
            }
        }
    }


    // 레시피 페이지 좌우 버튼 누를 시 호출
    public void UpdatePageIndexButton(bool isLeft)
    {
        // 인덱스 수 좌우 맞춰주기
        if (isLeft)
        {
            if (nowPageIndex > 0)
                nowPageIndex--;
            else return;
        }
        else
        {
            if (nowPageIndex < maxPageIndex)
                nowPageIndex++;
            else return;
        }

        // 변화한 페이지에 맞춰 레시피 슬롯 업데이트
        UpdateRecipePageData();
    }
   

    
}
