using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeListPage : MonoBehaviour
{
    [SerializeField] Transform recipeListParent;
    private RecipeSlotView[] recipeSlotViews;

    List<Item_Recipe> nowPageRecipeList = new List<Item_Recipe>();
    public int pageIndex;


    public void Init()
    {
        // ������ ���� ����Ʈ �ʱ�ȭ
        recipeSlotViews = recipeListParent.GetComponentsInChildren<RecipeSlotView>();
        // ���� ������ �ε����� �ִ� ������ ����Ʈ(ó���� 15�� ���� null)
        for (int i = 0; i < 15; i++)
        {
            nowPageRecipeList.Add(null);
        }
    }

    public void SetCurrentPageRecipeList(int start, int end)
    {
        Item_Recipe[] allRecipeList = BaseCampManager.Instance.baseCampData.allRecipeList;

        for (int i = start; i < end; i++)
        {
            if (allRecipeList.Length <= i)
            {
                nowPageRecipeList[i - pageIndex * i] = null;
            }
            else
            {
                nowPageRecipeList[i - pageIndex * i] = allRecipeList[i];
            }
        }
    }

    // ���� ������ �ε����� �´� ������ ������ => ������
    public void UpdateRecipePageData()       
    {

        if (pageIndex == 0)
        {
            SetCurrentPageRecipeList(0, 15);
        }
        else if (pageIndex == 1)
        {
            SetCurrentPageRecipeList(15, 30);
        }

        for (int i = 0; i < recipeSlotViews.Length; i++)
        {
            if (nowPageRecipeList[i] != null)
            {
                recipeSlotViews[i].gameObject.SetActive(true);

                // �����ִ� �����Ƕ�� => UI�� ������ ��������Ʈ �ֱ�
                if (nowPageRecipeList[i].RecipeData.isUnlocked)
                {
                    recipeSlotViews[i].recipeItem = nowPageRecipeList[i];
                    recipeSlotViews[i].SetRecipeSprite();
                }
                // ����ִ� �����Ƕ�� => UI�� ���� â���� ����
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

   
}
