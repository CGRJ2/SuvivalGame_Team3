using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [HideInInspector] public StageData[] stageDatas;
    [Header("현재 스테이지 단계")]
    public ObservableProperty<int> CurrentStageLevel = new ObservableProperty<int>();

    [Header("파밍오브젝트 리스폰 시간")]
    [SerializeField] int respawnTime_FO = 2;

    [Header("몬스터 리스폰 시간")]
    [SerializeField] int respawnTime_Monster = 2;

    [Header("거실 스포너 정보")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_LivingRoom;
    [Header("서재 스포너 정보")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_Library;
    [Header("옷장 스포너 정보")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_DressRoom;
    [Header("안방 스포너 정보")]
    [SerializeField] private SpawnerListGroup spawnerListGroup_MasterBedRoom;

    public SpawnerListGroup GetSpawnerListGroup(StageKey stageKey)
    {
        switch (stageKey)
        {
            case StageKey.거실:
                return spawnerListGroup_LivingRoom;
            case StageKey.서재:
                return spawnerListGroup_Library;
            case StageKey.옷방:
                return spawnerListGroup_DressRoom;
            case StageKey.안방:
                return spawnerListGroup_MasterBedRoom;
            default: return null;
        }
    }
    //////////////////////테스트/////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawningRoutinesStart(spawnerListGroup_LivingRoom);
        }
    }

    private void OnDisable()
    {
        // 타이틀씬으로 넘어가거나 종료 시, 코루틴 중지
        StopAllCoroutines();
    }
    //////////////////////테스트/////////////////////////////////


    // 파밍오브젝트 & 몬스터 리스폰 루틴 동시 실행
    public void SpawningRoutinesStart(SpawnerListGroup target)
    {
        // 파밍오브젝트 인스턴스 리스폰 루틴 실행
        StartCoroutine(
            SpawningRoutine(target.spawnerList_FO, target.activateInstances_FO, target.maxCount_FO, respawnTime_FO));

        // 몬스터 인스턴스 리스폰 루틴 실행
        StartCoroutine(
            SpawningRoutine(target.spawnerList_Monster, target.activateInstances_Monster, target.maxCount_Monster, respawnTime_Monster));
    }

    // 리스폰 시스템 어떻게 구성할지 고민 좀 해보자
    private IEnumerator SpawningRoutine(List<Spawner> targetSpawnerList, List<GameObject> activeInstanceList, int maxCount, float respawnTime)
    {
        while (true)
        {
            // 활성화된 오브젝트들의 수가 maxCount를 넘어가면 일시 정지
            yield return new WaitUntil(() => maxCount > activeInstanceList.Count);

            // 추가 소환 가능한 상태일 때 진행
            List<Spawner> notStartedList = GetNotStartedSpawnerList(targetSpawnerList);
            List<Spawner> progressingList = GetProgressingSpawnerList(targetSpawnerList);


            // 현재 소환 가능한 수량 = 최대 수량 - 현재 소환된 수량
            int spawnableCount = maxCount - activeInstanceList.Count;

            // 소환 대기 중인 애들의 수가 앞으로 소환할 수 있는 수량보다 적으면 => 추가 소환 가능
            if (progressingList.Count < spawnableCount)
            {
                RandomSpawnerStartSpawning(notStartedList,maxCount, respawnTime);
            }

            yield return null;
        }
    }


    // 아직 소환 코루틴이 진행되지 않은 스포너 리스트 반환
    private List<Spawner> GetNotStartedSpawnerList(List<Spawner> spawnerList)
    {
        List<Spawner> notStartedSpawnerList = new List<Spawner>();
        foreach (Spawner spawner in spawnerList)
        {
            // 리스폰 코루틴이 없고 && 이미 소환된 인스턴스를 보유하지 않은 스포너들
            if (spawner.spawnRoutineInProgress == null && spawner.currentSpawned_Instance == null)
                notStartedSpawnerList.Add(spawner);
        }
        return notStartedSpawnerList;
    }

    // 진행중인 리스폰 코루틴이 있는 스포너 리스트 반환
    private List<Spawner> GetProgressingSpawnerList(List<Spawner> spawnerList)
    {
        // 리스폰이 진행중인 애들
        List<Spawner> progressingSpawnerList = new List<Spawner>();
        foreach (Spawner spawner in spawnerList)
        {
            // 리스폰 코루틴이 존재하는 애들
            if (spawner.spawnRoutineInProgress != null)
                progressingSpawnerList.Add(spawner);
        }
        return progressingSpawnerList;
    }


    // 스폰 가능한 리스트를 받아, 그 들 중 랜덤으로 하나 선택해서 리스폰 루틴 실행
    private void RandomSpawnerStartSpawning(List<Spawner> spawnableList, int maxCount, float respawnTime)
    {
        // 대기 중인 스포너들 중 랜덤 선택
        if (spawnableList.Count == 0) { Debug.LogError("모든 스포너에 인스턴스가 생성되어있음"); return; }

        int r = UnityEngine.Random.Range(0, spawnableList.Count);
        spawnableList[r].StartSpawning(respawnTime);
    }


    public void Init()
    {
        base.SingletonInit();

        // 스테이지 정보 초기화
        InitStageDatas();
    }

    private void OnDestroy()
    {
        CurrentStageLevel.UnsbscribeAll();

    }

    // 스테이지 데이터들 초기화하기
    private void InitStageDatas()
    {
        stageDatas = Resources.LoadAll<StageData>("StageDatas");
        Array.Sort(stageDatas, (a, b) => a.StageLevel.CompareTo(b.StageLevel));

        for (int i = 0; i < stageDatas.Length; i++)
        {
            stageDatas[i].Init();
        }
    }


    // 스테이지가 해금될 때마다 CurrentStageLevel 업데이트
    public void SetCurrentStageIndex(int stageLevel)
    {
        CurrentStageLevel.Value = stageLevel;
    }



    #region [세이브 & 로드 데이터] 1. 스테이지 언락 여부 2. 스포너 상태 및 소환중인 오브젝트 정보

    public List<bool> GetStageUnlockSaveData()
    {
        // 언락 여부만 리스트 순서대로 저장
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
    거실, 서재, 옷방, 안방, All
}

[System.Serializable]
public class SpawnerListGroup
{
    [Header("현재 지역에 존재 가능한 파밍오브젝트 최대 갯수")]
    public int maxCount_FO = 10;
    public List<Spawner> spawnerList_FO = new List<Spawner>();
    public List<GameObject> activateInstances_FO = new List<GameObject>();

    [Header("현재 지역에 존재 가능한 몬스터 최대 마릿 수")]
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
