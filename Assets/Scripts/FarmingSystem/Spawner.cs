using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 종류 설정
    public SpawnerType spawnerType;

    // 해당 스포너가 존재할 장소
    public StageData stageOrigin;

    // 현재 소환된 파밍오브젝트.. 소환 안되어 있는 상태면 null로
    public GameObject currentSpawned_Instance;


    // 리스폰 루틴 상태
    public Coroutine spawnRoutineInProgress;

    private void Awake() => InitToList();

    private void InitToList()
    {
        StageManager sm = StageManager.Instance;
        sm.GetSpawnerListGroup(stageOrigin.stageKey).AddToSpawnerList(this);
    }

    public void Init()
    {
        DestroySpawnedObject();
        CancelSpawning();
    }

    private void OnDisable()
    {
        CancelSpawning();
    }

    private SpawnTable GetSpawnTable()
    {
        // 1. 스테이지 데이터의 언락 여부 판단
        if (!stageOrigin.IsUnlocked) return null;

        // 2. 현재 스테이지의 레벨에 따른 테이블 Weight 정보 확인
        if (spawnerType == SpawnerType.FO)
        {
            if (stageOrigin.SpawnTableGroup_FO.Count < 0) return null;
        }
        else if (spawnerType == SpawnerType.Monster)
        {
            if (stageOrigin.SpawnTableGroup_Monster.Count < 0) return null;
        }


        // 3. Origin이 거실인지, 아닌지 확인
        List<TableInfoByWeight> tableInfos = null;
        // 거실이라면 스테이지 레벨에 맞는 테이블 가중치 적용
        if (stageOrigin.stageKey == StageKey.거실)
        {
            int currentStageLevel = StageManager.Instance.CurrentStageLevel;

            if (spawnerType == SpawnerType.FO)
            {
                tableInfos = stageOrigin.SpawnTableGroup_FO[currentStageLevel].tableWeightInfos;
            }
            else if (spawnerType == SpawnerType.Monster)
            {
                tableInfos = stageOrigin.SpawnTableGroup_Monster[currentStageLevel].tableWeightInfos;
            }
        }
        // 거실이 아니라면 첫 번째 테이블 가중치 적용
        else
        {
            if (spawnerType == SpawnerType.FO)
            {
                tableInfos = stageOrigin.SpawnTableGroup_FO[0].tableWeightInfos;
            }
            else if (spawnerType == SpawnerType.Monster)
            {
                tableInfos = stageOrigin.SpawnTableGroup_Monster[0].tableWeightInfos;
            }
        }

        // 전체 가중치 설정
        int totalWeight = 0;
        int key = 0;
        foreach (TableInfoByWeight twi in tableInfos)
        {
            totalWeight += twi.weight;
        }

        // 확률 키 설정
        int r = Random.Range(0, totalWeight);



        // 확률에 따른 테이블 선택
        foreach (TableInfoByWeight twi in tableInfos)
        {
            key += twi.weight;
            if (r < key)
            {
                return twi.spawnTable;
            }
        }

        return null;
    }

    private GameObject GetSpawnStandByObject()
    {
        SpawnTable instanceSpawnTable = GetSpawnTable();
        if (instanceSpawnTable == null)
        {
            return null;
        }

        // 전체 가중치 설정
        int totalWeight = 0;
        int key = 0;
        foreach (SpawnObjectInfo soi in instanceSpawnTable.objectList)
        {
            totalWeight += soi.objectWeight;
        }

        // 확률 키 설정
        int r = Random.Range(0, totalWeight);

        // 확률에 따른 테이블 선택
        foreach (SpawnObjectInfo soi in instanceSpawnTable.objectList)
        {
            key += soi.objectWeight;
            if (r < key)
            {
                return soi.spawnObject;
            }
        }

        return null;
    }

    private void Spawn()
    {
        GameObject selectedPrefabs = GetSpawnStandByObject();

        if (selectedPrefabs != null)
        {
            // 인스턴스 소환
            GameObject instance = Instantiate(selectedPrefabs, transform);
            ISpawnable spawnable = instance.GetComponent<ISpawnable>();

            // 예외처리
            if (spawnable == null) { Debug.LogError("ISpawnable 인터페이스가 없는 오브젝트를 소환하려함!"); return; }

            // 해당 리스트 그룹에 활성된 오브젝트 리스트 찾기
            SpawnerListGroup tartgetSpawnerListGroup = StageManager.Instance.GetSpawnerListGroup(stageOrigin.stageKey);
            List<GameObject> targetActiveList = null;
            if (spawnerType == SpawnerType.FO)
                targetActiveList = tartgetSpawnerListGroup.activateInstances_FO;
            else if (spawnerType == SpawnerType.Monster)
                targetActiveList =  tartgetSpawnerListGroup.activateInstances_Monster;

            // 활성된 오브젝트 리스트에 인스턴스 추가
            if (targetActiveList != null)
                targetActiveList.Add(instance);
            
            currentSpawned_Instance = instance;

            // 해당 오브젝트 파괴(or 비활성화) 시 실행될 Action에 (활성된 오브젝트 리스트에 인스턴스 제거) 함수 저장
            spawnable.DeactiveAction = () =>
            {
                spawnable.OriginTransform = transform;
                targetActiveList.Remove(instance);
                currentSpawned_Instance = null;
            };
        }
        else
        {
            Debug.LogError($"선택된 오브젝트 프리펩이 없음! 오류 발생 위치 : {gameObject.name}({spawnerType}타입 스포너)");
        }
    }

    // 스포너 개개인 리스폰 매커니즘
    private IEnumerator SpawnRoutine(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        Spawn();

        // 스폰 완료 시 코루틴 삭제
        spawnRoutineInProgress = null;
    }

    public void StartSpawning(float respawnTime)
    {
        if (spawnRoutineInProgress == null)
            spawnRoutineInProgress = StartCoroutine(SpawnRoutine(respawnTime));
    }

    public void CancelSpawning()
    {
        if (spawnRoutineInProgress != null)
        {
            StopCoroutine(spawnRoutineInProgress);
            spawnRoutineInProgress = null;
        }
    }

    public void DestroySpawnedObject()
    {
        if (currentSpawned_Instance != null) Destroy(currentSpawned_Instance);
    }
}
public enum SpawnerType { FO, Monster }
