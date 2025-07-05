using System;
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerManager.Instance.PlayerFaint(-99);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerManager.Instance.PlayerDead();
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            UseTempCampItem();
        }
        
    }


    public void Init()
    {
        base.SingletonInit();
        baseCampData = new BaseCampData();
        tempCampData = new TempCampData();
        InitUpgradeConditions();
        InitAllRecipes();

        TemporaryCampInstance_Prefab = Resources.Load<GameObject>("Prefabs/BaseCamp/TempBaseCampInstance");

        DataManager.Instance.loadedDataGroup.Subscribe(LoadCampData);
    }

    // 세이브 용 임시텐트 데이터
    public TempCampData GetTempCampData()
    {
        if (currentTempCampInstance == null) return new TempCampData();
        else return new TempCampData(currentTempCampInstance.transform);
    }

    public void LoadCampData(SaveDataGroup loadedData)
    {
        baseCampData = loadedData.baseCampData;
        tempCampData = loadedData.tempCampData;

        // 기존 임시텐트 인스턴스 삭제
        if (currentTempCampInstance != null)
        {
            currentTempCampInstance.DestroyOnlyInstacne();
        }

        // 저장된 임시캠프 데이터가 있으면 임시텐트 인스턴스 생성
        if (tempCampData.isActive)
        {
            // 현재 리스폰 포인트(이전에 저장된 간이 캠프 위치)에 간이 캠프 프리펩 소환
            SpawnTempBaseCampInstance(tempCampData.tempCampPosition, tempCampData.tempCampRotation);
        }

        // 마지막 저장 캠프로 위치 이동
        MoveToLastCamp(true);

        Debug.Log("베이스캠프 매니저 데이터 구독자 함수 완료");
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

    public GameObject SpawnTempBaseCampInstance(Vector3 spawningPos, Quaternion spawningQuat)
    {
        GameObject tempCampInstance = Instantiate(TemporaryCampInstance_Prefab, spawningPos, spawningQuat);

        // 먼저 다른 임시캠프가 있다면 삭제
        if (currentTempCampInstance != null)
            currentTempCampInstance.DestroyOnlyInstacne();

        // 현재 소환한 임시캠프 지정
        if (tempCampInstance.GetComponent<TemporaryCampInstance>() == null) { Debug.LogError("TemporaryCampInstance가 없는 애를 프리펩으로 설정함"); }
        currentTempCampInstance = tempCampInstance.GetComponent<TemporaryCampInstance>();

        // 즉시 저장
        Debug.Log("간이 캠프 생성 => 데이터 즉시 저장");
        DataManager.Instance.SaveData(0);

        return tempCampInstance;
    }


    // 마지막으로 저장된 캠프로 이동
    public void MoveToLastCamp(bool isLoad)
    {
        Debug.Log("플레이어 강제 이동 판정");

        // 간이 캠프 데이터가 있다면 
        if (currentTempCampInstance != null)
        {
            Debug.Log($"간이캠프로 이동 => 위치 : {currentTempCampInstance.respawnPoint}");
            PlayerManager.Instance.instancePlayer.Respawn(currentTempCampInstance.respawnPoint);

            // 데이터 로드로 진행된 이동이 아니라면 -> 간이캠프 소모(파괴 & 데이터 비우기)
            if (!isLoad)
                currentTempCampInstance.DestroyWithData();
        }
        // 없으면 베이스캠프로 이동
        else
        {
            Debug.Log($"베이스 캠프로 이동 => 위치 : {baseCampInstance.respawnPoint}");
            PlayerManager.Instance.instancePlayer.Respawn(baseCampInstance.respawnPoint);
        }
    }

    public void UseTempCampItem()
    {
        // 생성
        Transform playerTransform = PlayerManager.Instance.instancePlayer.transform;
        GameObject gameObject = SpawnTempBaseCampInstance(playerTransform.position, playerTransform.rotation);
        gameObject.transform.SetParent(null);

        
    }



}

