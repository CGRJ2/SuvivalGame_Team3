using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    [field: SerializeField] public int MaxLevel { get; private set; }

    // [세이브 & 로드 데이터]
    public BaseCampData baseCampData;

    // [세이브 & 로드 데이터]
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
        // 로드 시 
        // currentCampData = 세이브 데이터의 Basc Camp Data 할당;
        // 스크립터블 오브젝트들(레시피 데이터 언락정보, 스테이지 데이터 클리어 여부 등)은 자동으로 저장되니 동기화 안해도 괜찮음
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
        else Debug.Log("레벨업 불가 [사유 : 이미 최대 레벨입니다.]");
    }


}
