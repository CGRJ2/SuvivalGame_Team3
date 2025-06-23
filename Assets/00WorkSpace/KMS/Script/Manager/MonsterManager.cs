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
                SpawnMonster(spawnInfo.monsterPrefab);
            }
        }
    }

    // 몬스터 생성 (랜덤)
    private void SpawnMonster(BaseMonster prefab)
    {
        int rand = Random.Range(0, spawnPoints.Length);
        Transform spawnPos = spawnPoints[rand];
        BaseMonster monster = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
        RegisterMonster(monster);
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
    private IEnumerator SpawnMonstersOverTime(WaveSpawnInfo spawnInfo)
    {
        for (int i = 0; i < spawnInfo.count; i++)
        {
            SpawnMonster(spawnInfo.monsterPrefab, spawnInfo.spawnPosition);
            yield return new WaitForSeconds(spawnInfo.spawnDelay);
        }
    }
    // 몬스터 생성 (지정위치 EX : 보스, 주인, 고양이)
    private void SpawnMonster(BaseMonster prefab, Vector3 position)
    {
        BaseMonster monster = Instantiate(prefab, position, Quaternion.identity);
        RegisterMonster(monster);
    }
}
