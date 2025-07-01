using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    [field: SerializeField] public int MaxLevel { get; private set; }

    public BaseCampData currentCampData;

    [HideInInspector] public Item_Recipe[] allRecipeList;

    [HideInInspector] public BaseCampUpgradeCondition[] UpgradeConditions;

    public void Init()
    {
        base.SingletonInit();
        currentCampData = new BaseCampData();
        InitUpgradeConditions();
        InitAllRecipes();
    }

    public void LoadData()
    {
        // �ε� �� 
        // currentCampData = ���̺� �������� Basc Camp Data �Ҵ�;
        // ��ũ���ͺ� ������Ʈ��(������ ������ �������, �������� ������ Ŭ���� ���� ��)�� �ڵ����� ����Ǵ� ����ȭ ���ص� ������
    }


    private void InitUpgradeConditions()
    {
        UpgradeConditions = Resources.LoadAll<BaseCampUpgradeCondition>("BaseCampUpgradeConditions");
        Array.Sort(UpgradeConditions, (a, b) => a.currentLevel.CompareTo(b.currentLevel));
    }

    public void InitAllRecipes()
    {
        allRecipeList = Resources.LoadAll<Item_Recipe>("ItemDatabase/99 Recipes");
        Array.Sort(allRecipeList, (a, b) => a.RecipeData.orderIndex.CompareTo(b.RecipeData.orderIndex));
    }

    public void LevelUp()
    {
        if (currentCampData.CurrentCampLevel.Value < MaxLevel)
            currentCampData.CurrentCampLevel.Value += 1;
        else Debug.Log("������ �Ұ� [���� : �̹� �ִ� �����Դϴ�.]");
    }

}
