using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }

    [Header("몬스터 관련 데이터")]
    [SerializeField] private List<WaveData> waveDataList; // 몬스터 생성기반이 Time기준이면 WaveData 수정 필요
    [SerializeField] private Transform[] spawnPoints;
    //[SerializeField] private timeManager TimeManager; // 타임매니저 생성시 주석 해제 필요

    private int currentWave = 0;
    private List<BaseMonster> activeMonsters = new List<BaseMonster>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // 활성화된 몬스터 목록에 등록
    public void RegisterMonster(BaseMonster monster)
    {
        if (!activeMonsters.Contains(monster))
            activeMonsters.Add(monster);
    }

    // 활성화된 몬스터 목록에서 제거
    public void UnregisterMonster(BaseMonster monster) //몬스터 
    {
        if (activeMonsters.Contains(monster))
            activeMonsters.Remove(monster);
    }

    // 현재 활성 상태인 몬스터 수 반환
    public int GetAliveMonsterCount()
    {
        return activeMonsters.Count;
    }

    // 웨이브 시작 
    public void StartWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveDataList.Count) return;
        currentWave = waveIndex;

        WaveData data = waveDataList[waveIndex];
        foreach (var spawnInfo in data.spawnInfos)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                Vector3 spawnPos = spawnInfo.spawnPosition != Vector3.zero
                    ? spawnInfo.spawnPosition
                    : GetRandomSpawnPosition();

                var monster = MonsterFactory.Instance.SpawnMonster(spawnInfo.monsterData, spawnPos);
                if (monster != null)
                    RegisterMonster(monster);
            }
        }
    }
    // 몬스터 생성 시작 시간
    public void StartWaveTimed(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waveDataList.Count) return;
        currentWave = waveIndex;

        WaveData data = waveDataList[waveIndex];
        foreach (var spawnInfo in data.spawnInfos)
        {
            StartCoroutine(SpawnMonstersOverTime(spawnInfo));
        }
    }

    // 생성 지연 시간
    private IEnumerator SpawnMonstersOverTime(WaveSpawnInfo info)
    {
        for (int i = 0; i < info.count; i++)
        {
            Vector3 spawnPos = info.spawnPosition != Vector3.zero
                ? info.spawnPosition
                : GetRandomSpawnPosition();

            var monster = MonsterFactory.Instance.SpawnMonster(info.monsterData, spawnPos);
            if (monster != null)
                RegisterMonster(monster);

            yield return new WaitForSeconds(info.spawnDelay);
        }
    }

    // 랜덤 생성
    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[MonsterManager] 스폰 포인트가 비어있음");
            return transform.position; // fallback
        }

        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].position;
    }
}
