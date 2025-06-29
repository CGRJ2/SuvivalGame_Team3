using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_BaseCampData
{
    public ObservableProperty<int> CurrentCampLevel { get; private set; }
    // ���� ���� ������ ���� (���� ����� ������, bool isUnlocked, ��� Ʈ���� ������(������) => �� �������� �����ؼ� ���()�ϸ� �������..)

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
        else Debug.Log("������ �Ұ� [���� : �̹� �ִ� �����Դϴ�.]");
    }
    

    // ����� �����ǵ鸸 ����
    public List<Item_Recipe> GetUnlockRecipeList()
    {
        List<Item_Recipe> unlockedRecipes = new List<Item_Recipe>();

        foreach (Item_Recipe recipe in allRecipeList)
        {
            // ������ ����Ʈ �ȿ��� (������ �̸� == ����� ������ ������ �̸�) �� ��� ������ ���
            if (recipe.RecipeData.isUnlocked)
            {
                unlockedRecipes.Add(recipe);
            }
        }

        // �ε��� ������ ������
        unlockedRecipes.Sort((a, b) => a.RecipeData.orderIndex.CompareTo(b.RecipeData.orderIndex));

        return unlockedRecipes;
    }
}
