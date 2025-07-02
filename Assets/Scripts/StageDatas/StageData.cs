using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "New StageData", menuName = "New StageData")]
public class StageData : ScriptableObject
{
    [field: SerializeField] public bool IsUnlocked { get; private set; }
    [field: SerializeField] public int StageLevel { get; private set; }
    [field: SerializeField] public Sprite StageImage { get; private set; }
    [field: SerializeField] public string StageName { get; private set; }

    public StageKey stageKey;

    [Header("스테이지 언락 조건")]
    public StageUnlockCondition unlockCondition;


    [Header("파밍오브젝트 스폰 데이터 (레벨에 따른 분류)")]
    public List<TableInfosByStageLevel> SpawnTableGroup_FO;

    [Header("몬스터 스폰 데이터 (레벨에 따른 분류)")]
    public List<TableInfosByStageLevel> SpawnTableGroup_Monster;






    protected void OnEnable()
    {
        StageName = this.name;
        unlockCondition.needTimeState = new List<TimeZoneState>() { TimeZoneState.All };
    }

    public void Init()
    {
        //Init_FO_List();
        //Init_Monster_List();
        InitUnlockState();
    }

    public void UlockStage()
    {
        IsUnlocked = true;
        StageManager.Instance.SetCurrentStageIndex(StageLevel);
    }

    public void InitUnlockState()
    {
        if (stageKey == StageKey.거실) IsUnlocked = true;
        else IsUnlocked = false;
    }
}

[System.Serializable]
public class StageUnlockCondition
{
    public List<ItemRequirement> needItemList;
    public List<TimeZoneState> needTimeState;
}

[System.Serializable]
public class TableInfosByStageLevel
{
    [Header("Table 그룹 내부, 각 Table 데이터 분류(가중치에 의한 분류)")]
    public List<TableInfoByWeight> tableWeightInfos;
}

[System.Serializable]
public class TableInfoByWeight
{
    [Header("테이블 데이터 & 확률 가중치")]
    public SpawnTable spawnTable;
    public int weight;
}


