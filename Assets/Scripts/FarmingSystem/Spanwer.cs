using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spanwer : MonoBehaviour
{
    // 현재 소환된 파밍오브젝트.. 소환 안되어 있는 상태면 null로
    public GameObject currentSpawned_Instance;

    // 해당 스포너가 존재할 장소
    public StageData stageOrigin;

    ////////////////////////////
    ///오늘 할거
    ///스포너들 초기화 시 => 스테이지 매니저 안에 현재 스포너들 리스트에 넣고!
    ///스포너들은 스폰될 파밍오브젝트를 자신의 Origin지역에 맞는 StageData의 farmingObjectList를 받아 그 안에서 랜덤으로 오브젝트 하나 지정
    ///스포너 리스트들 중에서 랜덤으로 스폰하게 만들기


    //////////////////////테스트/////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spawn();
        }
    }
    //////////////////////테스트/////////////////////////////////


    private SpawnTable GetSpawnTable()
    {
        Debug.Log("0");

        // 1. 스테이지 데이터의 언락 여부 판단
        if (!stageOrigin.IsUnlocked) return null;
        Debug.Log("1");

        // 2. 현재 스테이지의 레벨에 따른 테이블 Weight 정보 확인
        if (stageOrigin.SpawnTableGroup_FO.Count < 0) return null;
        Debug.Log("2");

        // 3. Origin이 거실인지, 아닌지 확인
        List<TableInfoByWeight> tableInfos = null;
        // 거실이라면 스테이지 레벨에 맞는 테이블 가중치 적용
        if (stageOrigin.stageKey == StageKey.거실)
        {
            int currentStageLevel = StageManager.Instance.CurrentStageLevel.Value;
            tableInfos = stageOrigin.SpawnTableGroup_FO[currentStageLevel].tableWeightInfos;
        }
        // 거실이 아니라면 첫 번째 테이블 가중치 적용
        else
        {
            tableInfos = stageOrigin.SpawnTableGroup_FO[0].tableWeightInfos;
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

        Debug.Log("확률 계산 전에는 오는지");


        // 확률에 따른 테이블 선택
        foreach (TableInfoByWeight twi in tableInfos)
        {
            key += twi.weight;
            if (r < key)
            {
                Debug.Log("스폰 테이블 줌");
                return twi.spawnTable;
            }
        }

        return null;
    }

    public GameObject GetSpawnStandByObject()
    {
        SpawnTable instanceSpawnTable = GetSpawnTable();
        if (instanceSpawnTable == null)
        {
            Debug.LogError("스폰테이블이 없습니다");
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

    public void Spawn()
    {
        GameObject instance = GetSpawnStandByObject();
        if (instance != null)
            currentSpawned_Instance = Instantiate(instance, transform.position, transform.rotation);
    }
}
