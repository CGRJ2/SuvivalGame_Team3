using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [HideInInspector] public StageData[] stageDatas;
    [Header("���� �������� �ܰ�")]
    public int CurrentStageLevel;

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


    public void Init()
    {
        base.SingletonInit();

        // �������� ���� �ʱ�ȭ
        InitStageDatas();
        SetCurrentStageIndex(0);
        SpawningRoutinesStartByCurrentLevel(CurrentStageLevel);


        // ������ �ε��� �� �������� �� ��� ������ �ε��� ��� �������ֱ�
        DataManager dm = DataManager.Instance;
        dm.loadedDataGroup.Subscribe(LoadStageUnlockSaveData);
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

    private void OnDisable()
    {
        // Ÿ��Ʋ������ �Ѿ�ų� ���� ��, �ڷ�ƾ ����
        StopAllCoroutines();
    }

    // ���������� �رݵ� ������ CurrentStageLevel ������Ʈ
    public void SetCurrentStageIndex(int stageLevel)
    {
        if (stageLevel <= CurrentStageLevel) return;
        CurrentStageLevel = stageLevel;
        SpawningRoutinesStartByCurrentLevel(CurrentStageLevel);
    }

    // �������� Ű�� ���� SpawnerListGroup ��ȯ (�����ʵ� ������ �ʱ�ȭ �� Key�� ��ġ�ϴ� �׷����� �ֱ� ����)
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

    // CurrentLevel�� ��ȭ�� ����, �ش� ���������� �����׷�ƾ �ر�
    public void SpawningRoutinesStartByCurrentLevel(int currentLevel)
    {
        switch (currentLevel)
        {
            case 0:
                Debug.LogWarning("0�������� ���� ����Ŭ ����!");
                SpawningRoutinesStart(spawnerListGroup_LivingRoom);
                break;

            case 1:
                Debug.LogWarning("1�������� ���� ����Ŭ ����!");
                SpawningRoutinesStart(spawnerListGroup_Library);
                break;

            case 2:
                Debug.LogWarning("2�������� ���� ����Ŭ ����!");
                SpawningRoutinesStart(spawnerListGroup_DressRoom);
                break;

            case 3:
                Debug.LogWarning("3�������� ���� ����Ŭ ����!");
                SpawningRoutinesStart(spawnerListGroup_MasterBedRoom);
                break;

            default:
                break;
        }
    }

    // �Ĺֿ�����Ʈ & ���� ������ ��ƾ ���� ����
    private void SpawningRoutinesStart(SpawnerListGroup target)
    {
        // �Ĺֿ�����Ʈ �ν��Ͻ� ������ ��ƾ ����
        StartCoroutine(
            SpawningRoutine(target.spawnerList_FO, target.activateInstances_FO, target.maxCount_FO, respawnTime_FO));

        // ���� �ν��Ͻ� ������ ��ƾ ����
        StartCoroutine(
            SpawningRoutine(target.spawnerList_Monster, target.activateInstances_Monster, target.maxCount_Monster, respawnTime_Monster));
    }
    private IEnumerator SpawningRoutine(List<Spawner> targetSpawnerList, List<GameObject> activeInstanceList, int maxCount, float respawnTime)
    {
        int firstInitCount = maxCount / 2;

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
                if (firstInitCount > 0)
                {
                    RandomSpawnerStartSpawning(notStartedList, maxCount, 0.2f);
                    firstInitCount--;
                }
                else
                {
                    RandomSpawnerStartSpawning(notStartedList, maxCount, respawnTime);
                }
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

    public void LoadStageUnlockSaveData(SaveDataGroup saveDataGroup)
    {
        // 1-1. ������ �ڷ�ƾ �ʱ�ȭ
        StopAllCoroutines();

        // 1-2. ���� ������ ���� �ʱ�ȭ & ���� ��ü�� ��ü ����
        List<GameObject> activedAllObjects = new List<GameObject>();
        activedAllObjects.AddRange(spawnerListGroup_LivingRoom.activateInstances_FO);
        activedAllObjects.AddRange(spawnerListGroup_LivingRoom.activateInstances_Monster);
        activedAllObjects.AddRange(spawnerListGroup_Library.activateInstances_Monster);
        activedAllObjects.AddRange(spawnerListGroup_Library.activateInstances_Monster);
        activedAllObjects.AddRange(spawnerListGroup_DressRoom.activateInstances_Monster);
        activedAllObjects.AddRange(spawnerListGroup_DressRoom.activateInstances_Monster);
        activedAllObjects.AddRange(spawnerListGroup_MasterBedRoom.activateInstances_Monster);
        activedAllObjects.AddRange(spawnerListGroup_MasterBedRoom.activateInstances_Monster);

        foreach (GameObject gameObject in activedAllObjects)
        {
            // ������ƮǮ�� �ٲ�ߵ�
            Destroy(gameObject);
        }


        // 2-1. �������� ���� �ʱ�ȭ & 0�ܰ� �ڷ�ƾ ����
        CurrentStageLevel = 0;
        SpawningRoutinesStartByCurrentLevel(CurrentStageLevel);

        // 2-2. �������� ������ ��� ������ ����ȭ
        List<bool> loadedData = saveDataGroup.stageUnlockData;

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

    public void InitSpawners()
    {
        foreach (Spawner spawner in spawnerList_FO)
        {
            spawner.Init();
        }

        foreach (Spawner spawner in spawnerList_Monster)
        {
            spawner.Init();
        }
    }



}
