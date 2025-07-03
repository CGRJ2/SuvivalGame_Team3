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

    // ���� ������ �ε����� �´� ������ ������ => ������
    public void UpdateRecipePageData()
    {
        // �ӽ�
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


    // ������ ������ �¿� ��ư ���� �� ȣ��
    public void UpdatePageIndexButton(bool isLeft)
    {
        // �ε��� �� �¿� �����ֱ�
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

        // ��ȭ�� �������� ���� ������ ���� ������Ʈ
        UpdateRecipePageData();
    }
   

    
}
