using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [HideInInspector] public StageData[] stageDatas;
    [Header("���� �������� �ܰ�")]
    public ObservableProperty<int> CurrentStageLevel = new ObservableProperty<int>();

    [Header("�Ĺֿ�����Ʈ ������ �ð�")]
    [SerializeField] int respawnTime_FO = 2;

    [Header("���� ������ �ð�")]
    [SerializeField] int respawnTime_Monster = 2;

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
            SpawningRoutinesStart(spawnerListGroup_LivingRoom);
        }
    }

    private void OnDisable()
    {
        // Ÿ��Ʋ������ �Ѿ�ų� ���� ��, �ڷ�ƾ ����
        StopAllCoroutines();
    }
    //////////////////////�׽�Ʈ/////////////////////////////////


    // �Ĺֿ�����Ʈ & ���� ������ ��ƾ ���� ����
    public void SpawningRoutinesStart(SpawnerListGroup target)
    {
        // �Ĺֿ�����Ʈ �ν��Ͻ� ������ ��ƾ ����
        StartCoroutine(
            SpawningRoutine(target.spawnerList_FO, target.activateInstances_FO, target.maxCount_FO, respawnTime_FO));

        // ���� �ν��Ͻ� ������ ��ƾ ����
        StartCoroutine(
            SpawningRoutine(target.spawnerList_Monster, target.activateInstances_Monster, target.maxCount_Monster, respawnTime_Monster));
    }

    // ������ �ý��� ��� �������� ��� �� �غ���
    private IEnumerator SpawningRoutine(List<Spawner> targetSpawnerList, List<GameObject> activeInstanceList, int maxCount, float respawnTime)
    {
        while (true)
        {
            // Ȱ��ȭ�� ������Ʈ���� ���� maxCount�� �Ѿ�� �Ͻ� ����
            yield return new WaitUntil(() => maxCount > activeInstanceList.Count);

            // �߰� ��ȯ ������ ������ �� ����
            List<Spawner> notStartedList = GetNotStartedSpawnerList(targetSpawnerList);
            List<Spawner> progressingList = GetProgressingSpawnerList(targetSpawnerList);


            // ���� ��ȯ ������ ���� = �ִ� ���� - ���� ��ȯ�� ����
            int spawnableCount = maxCount - activeInstanceList.Count;

            // ��ȯ ��� ���� �ֵ��� ���� ������ ��ȯ�� �� �ִ� �������� ������ => �߰� ��ȯ ����
            if (progressingList.Count < spawnableCount)
            {
                RandomSpawnerStartSpawning(notStartedList,maxCount, respawnTime);
            }

            yield return null;
        }
    }


    // ���� ��ȯ �ڷ�ƾ�� ������� ���� ������ ����Ʈ ��ȯ
    private List<Spawner> GetNotStartedSpawnerList(List<Spawner> spawnerList)
    {
        List<Spawner> notStartedSpawnerList = new List<Spawner>();
        foreach (Spawner spawner in spawnerList)
        {
            // ������ �ڷ�ƾ�� ���� && �̹� ��ȯ�� �ν��Ͻ��� �������� ���� �����ʵ�
            if (spawner.spawnRoutineInProgress == null && spawner.currentSpawned_Instance == null)
                notStartedSpawnerList.Add(spawner);
        }
        return notStartedSpawnerList;
    }

    // �������� ������ �ڷ�ƾ�� �ִ� ������ ����Ʈ ��ȯ
    private List<Spawner> GetProgressingSpawnerList(List<Spawner> spawnerList)
    {
        // �������� �������� �ֵ�
        List<Spawner> progressingSpawnerList = new List<Spawner>();
        foreach (Spawner spawner in spawnerList)
        {
            // ������ �ڷ�ƾ�� �����ϴ� �ֵ�
            if (spawner.spawnRoutineInProgress != null)
                progressingSpawnerList.Add(spawner);
        }
        return progressingSpawnerList;
    }


    // ���� ������ ����Ʈ�� �޾�, �� �� �� �������� �ϳ� �����ؼ� ������ ��ƾ ����
    private void RandomSpawnerStartSpawning(List<Spawner> spawnableList, int maxCount, float respawnTime)
    {
        // ��� ���� �����ʵ� �� ���� ����
        if (spawnableList.Count == 0) { Debug.LogError("��� �����ʿ� �ν��Ͻ��� �����Ǿ�����"); return; }

        int r = UnityEngine.Random.Range(0, spawnableList.Count);
        spawnableList[r].StartSpawning(respawnTime);
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
    public List<GameObject> activateInstances_FO = new List<GameObject>();

    [Header("���� ������ ���� ������ ���� �ִ� ���� ��")]
    public int maxCount_Monster = 10;
    public List<Spawner> spawnerList_Monster = new List<Spawner>();
    public List<GameObject> activateInstances_Monster = new List<GameObject>();

    public void ActivedInstancesDataInit()
    {
        activateInstances_FO = new List<GameObject>();
        activateInstances_Monster = new List<GameObject>();
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
