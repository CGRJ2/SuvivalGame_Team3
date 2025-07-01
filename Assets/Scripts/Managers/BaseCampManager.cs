using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    [field: SerializeField] public int MaxLevel { get; private set; }

    // [���̺� & �ε� ������]
    public BaseCampData baseCampData;

    // [���̺� & �ε� ������]
    public TempCampData tempCampData;

    [HideInInspector] public Item_Recipe[] allRecipeList;

    [HideInInspector] public BaseCampUpgradeCondition[] UpgradeConditions;

    public BaseCampInstance baseCampInstance;
    public TemporaryCampInstance currentTempCampInstance;

    public void Init()
    {
        base.SingletonInit();
        baseCampData = new BaseCampData();
        tempCampData = null;
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
        if (baseCampData.CurrentCampLevel.Value < MaxLevel)
            baseCampData.CurrentCampLevel.Value += 1;
        else Debug.Log("������ �Ұ� [���� : �̹� �ִ� �����Դϴ�.]");
    }


}
