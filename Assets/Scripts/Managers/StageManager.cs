using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [HideInInspector] public StageData[] stageDatas;
    [Header("���� �������� �ܰ�")]
    public ObservableProperty<int> CurrentStageLevel = new ObservableProperty<int>();


    [Header("�Ž� ������ ����")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_LivingRoom;
    [Header("���� ������ ����")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_Library;
    [Header("���� ������ ����")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_DressRoom;
    [Header("�ȹ� ������ ����")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_MasterBedRoom;

    public SpawnerListGroup GetSpawnerListGroup(StageKey stageKey)
    {
        switch (stageKey)
        {
            case StageKey.�Ž�:
                return spawnerListGroup_LivingRoom;
            case StageKey.����:
                return spawnerListGroup_Library;
            case StageKey.�ʹ�:
                return spawnerListGroup_DressRoom;
            case StageKey.�ȹ�:
                return spawnerListGroup_MasterBedRoom;
            default: return null;
        }
    }
    //////////////////////�׽�Ʈ/////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnCycleTest(spawnerListGroup_LivingRoom);
        }
    }
    //////////////////////�׽�Ʈ/////////////////////////////////

    public void SpawnCycleTest(SpawnerListGroup targetSpawnerGroup)
    {
        // FO�ν��Ͻ� ���� ����
        // �ִ� �������� ���� �����Ǿ� �ִٸ�
        if (targetSpawnerGroup.activedFOInstances.Count < targetSpawnerGroup.maxCount_FO)
        {
            // �����ʵ� �� ���� ����
            int r = UnityEngine.Random.Range(0, targetSpawnerGroup.spawnerList_FO.Count - 1);
            targetSpawnerGroup.spawnerList_FO[r].Spawn();
        }
        else
        {
            Debug.Log("���� �� �� �ִ� �������� ����! �߰� �Ұ�");
        }
    }


    public void Init()
    {
        base.SingletonInit();

        // �������� ���� �ʱ�ȭ
        InitStageDatas();
    }

    private void OnDestroy()
    {
        CurrentStageLevel.UnsbscribeAll();

    }

    // �������� �����͵� �ʱ�ȭ�ϱ�
    private void InitStageDatas()
    {
        stageDatas = Resources.LoadAll<StageData>("StageDatas");
        Array.Sort(stageDatas, (a, b) => a.StageLevel.CompareTo(b.StageLevel));

        for (int i = 0; i < stageDatas.Length; i++)
        {
            stageDatas[i].Init();
        }
    }


    // ���������� �رݵ� ������ CurrentStageLevel ������Ʈ
    public void SetCurrentStageIndex(int stageLevel)
    {
        CurrentStageLevel.Value = stageLevel;
    }



    #region [���̺� & �ε� ������] 1. �������� ��� ���� 2. ������ ���� �� ��ȯ���� ������Ʈ ����

    public List<bool> GetStageUnlockSaveData()
    {
        // ��� ���θ� ����Ʈ ������� ����
        List<bool> instanceList = new List<bool>();
        foreach (StageData stageData in stageDatas)
        {
            instanceList.Add(stageData.IsUnlocked);
        }
        return instanceList;
    }

    public void LoadStageUnlockSaveData(List<bool> loadedData)
    {
        for (int i = 0; i < stageDatas.Length; i++)
        {
            if (loadedData[i]) stageDatas[i].UlockStage();
        }
    }

    #endregion

}

public enum StageKey
{
    �Ž�, ����, �ʹ�, �ȹ�, All
}

[System.Serializable]
public class SpawnerListGroup
{
    [Header("���� ������ ���� ������ �Ĺֿ�����Ʈ �ִ� ����")]
    public int maxCount_FO = 10;
    public List<Spawner> spawnerList_FO = new List<Spawner>();
    public List<GameObject> activedFOInstances = new List<GameObject>();

    [Header("���� ������ ���� ������ ���� �ִ� ���� ��")]
    public int maxCount_Monster = 10;
    public List<Spawner> spawnerList_Monster = new List<Spawner>();
    public List<GameObject> activedMonsterInstances = new List<GameObject>();

    public void ActivedInstancesDataInit()
    {
        activedFOInstances = new List<GameObject>();
        activedMonsterInstances = new List<GameObject>();
    }

    public void AddToSpawnerList(Spawner spawner)
    {
        if (spawner.spawnerType == SpawnerType.FO)
        {
            spawnerList_FO.Add(spawner);
        }
        else if (spawner.spawnerType == SpawnerType.Monster)
        {
            spawnerList_Monster.Add(spawner);
        }
    }


}
