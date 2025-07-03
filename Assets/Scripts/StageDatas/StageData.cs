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

    [Header("�������� ��� ����")]
    public StageUnlockCondition unlockCondition;


    [Header("�Ĺֿ�����Ʈ ���� ������ (������ ���� �з�)")]
    public List<TableInfosByStageLevel> SpawnTableGroup_FO;

    [Header("���� ���� ������ (������ ���� �з�)")]
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
        if (stageKey == StageKey.�Ž�) IsUnlocked = true;
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
    [Header("Table �׷� ����, �� Table ������ �з�(����ġ�� ���� �з�)")]
    public List<TableInfoByWeight> tableWeightInfos;
}

[System.Serializable]
public class TableInfoByWeight
{
    [Header("���̺� ������ & Ȯ�� ����ġ")]
    public SpawnTable spawnTable;
    public int weight;
}


