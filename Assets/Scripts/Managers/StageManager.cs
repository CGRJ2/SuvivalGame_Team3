using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [HideInInspector] public StageData[] stageDatas;
    [Header("현재 스테이지 단계")]
    public ObservableProperty<int> CurrentStageLevel = new ObservableProperty<int>();


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
            SpawnCycleTest(spawnerListGroup_LivingRoom);
        }
    }
    //////////////////////테스트/////////////////////////////////

    public void SpawnCycleTest(SpawnerListGroup targetSpawnerGroup)
    {
        // FO인스턴스 수량 관리
        // 최대 수량보다 적게 생성되어 있다면
        if (targetSpawnerGroup.activedFOInstances.Count < targetSpawnerGroup.maxCount_FO)
        {
            // 스포너들 중 랜덤 선택
            int r = UnityEngine.Random.Range(0, targetSpawnerGroup.spawnerList_FO.Count - 1);
            targetSpawnerGroup.spawnerList_FO[r].Spawn();
        }
        else
        {
            Debug.Log("현재 맵 상에 최대 수량으로 존재! 추가 불가");
        }
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
    public List<GameObject> activedFOInstances = new List<GameObject>();

    [Header("현재 지역에 존재 가능한 몬스터 최대 마릿 수")]
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
