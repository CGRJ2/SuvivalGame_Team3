using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    [field: SerializeField] public int MaxLevel { get; private set; }

    public BaseCampInstance baseCampInstance;

    public TemporaryCampInstance currentTempCampInstance;

    public GameObject TemporaryCampInstance_Prefab;

    // [세이브 & 로드 데이터]
    public BaseCampData baseCampData;

    // [세이브 & 로드 데이터]
    public TempCampData tempCampData;

    [HideInInspector] public Item_Recipe[] allRecipeList;

    [HideInInspector] public BaseCampUpgradeCondition[] UpgradeConditions;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(tempCampData);
        }
    }


    public void Init()
    {
        base.SingletonInit();
        baseCampData = new BaseCampData();
        tempCampData = new TempCampData(null);
        InitUpgradeConditions();
        InitAllRecipes();

        DataManager.Instance.loadedDataGroup.Subscribe(LoadCampData);
    }


    public void LoadCampData(SaveDataGroup loadedData)
    {
        baseCampData = loadedData.baseCampData;
        tempCampData = loadedData.tempCampData;
    }


    private void InitUpgradeConditions()
    {
        UpgradeConditions = Resources.LoadAll<BaseCampUpgradeCondition>("BaseCampUpgradeConditions");
        Array.Sort(UpgradeConditions, (a, b) => a.currentLevel.CompareTo(b.currentLevel));
    }

    private void InitAllRecipes()
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

    public GameObject SpawnTempBaseCampInstance(Transform spawningTransform)
    {
        return Instantiate(TemporaryCampInstance_Prefab, spawningTransform.position, spawningTransform.rotation);
    }
}

