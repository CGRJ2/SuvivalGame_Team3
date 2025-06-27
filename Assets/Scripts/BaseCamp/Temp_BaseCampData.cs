using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_BaseCampData
{
    public ObservableProperty<int> CurrentCampLevel { get; private set; }
    // ���� ���� ������ ���� (���� ����� ������, bool isUnlocked, ��� Ʈ���� ������(������) => �� �������� �����ؼ� ���()�ϸ� �������..)

    List<RecipeData> recipeList;

    public void LevelUp()
    {
        if (CurrentCampLevel.Value < BaseCampManager.Instance.MaxLevel)
            CurrentCampLevel.Value += 1;
        else Debug.Log("������ �Ұ� [���� : �̹� �ִ� �����Դϴ�.]");
    }
    

    // ������ ������ ��� ȿ���� �����Ϸ��� key�� �޾ƿͼ� ���� �� ���
    public void UnlockRecipe(string recipeItemName)
    {

        foreach (RecipeData recipe in recipeList)
        {
            // ������ ����Ʈ �ȿ��� (������ �̸� == ����� ������ ������ �̸�) �� ��� ������ ���
            if (recipe.recipeName == recipeItemName)
            {
                // �̹� ����Ǿ� �ִ� ���¶��
                if (recipe.isUnlocked)
                {
                    Debug.Log("�̹� �˰� �ִ� ���չ��̴�. ");
                }
                else
                {
                    recipe.isUnlocked = true;
                }
            }
        }
    }
}
