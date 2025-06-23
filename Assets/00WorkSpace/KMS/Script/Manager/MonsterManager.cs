using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }

    [Header("���� ���� ������")]
    [SerializeField] private List<WaveData> waveDataList; // ���� ��������� Time�����̸� WaveData ���� �ʿ�
    [SerializeField] private Transform[] spawnPoints;
    //[SerializeField] private timeManager TimeManager; // Ÿ�ӸŴ��� ������ �ּ� ���� �ʿ�

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

    // Ȱ��ȭ�� ���� ��Ͽ� ���
    public void RegisterMonster(BaseMonster monster)
    {
        if (!activeMonsters.Contains(monster))
            activeMonsters.Add(monster);
    }

    // Ȱ��ȭ�� ���� ��Ͽ��� ����
    public void UnregisterMonster(BaseMonster monster) //���� 
    {
        if (activeMonsters.Contains(monster))
            activeMonsters.Remove(monster);
    }

    // ���� Ȱ�� ������ ���� �� ��ȯ
    public int GetAliveMonsterCount()
    {
        return activeMonsters.Count;
    }

    // ���̺� ���� 
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

    // ���� ���� (����)
    private void SpawnMonster(BaseMonster prefab)
    {
        int rand = Random.Range(0, spawnPoints.Length);
        Transform spawnPos = spawnPoints[rand];
        BaseMonster monster = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
        RegisterMonster(monster);
    }

    // ���� ���� ���� �ð�
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

    // ���� ���� �ð�
    private IEnumerator SpawnMonstersOverTime(WaveSpawnInfo spawnInfo)
    {
        for (int i = 0; i < spawnInfo.count; i++)
        {
            SpawnMonster(spawnInfo.monsterPrefab, spawnInfo.spawnPosition);
            yield return new WaitForSeconds(spawnInfo.spawnDelay);
        }
    }
    // ���� ���� (������ġ EX : ����, ����, �����)
    private void SpawnMonster(BaseMonster prefab, Vector3 position)
    {
        BaseMonster monster = Instantiate(prefab, position, Quaternion.identity);
        RegisterMonster(monster);
    }
}
