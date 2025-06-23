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
                Vector3 spawnPos = spawnInfo.spawnPosition != Vector3.zero
                    ? spawnInfo.spawnPosition
                    : GetRandomSpawnPosition();

                var monster = MonsterFactory.Instance.SpawnMonster(spawnInfo.monsterData, spawnPos);
                if (monster != null)
                    RegisterMonster(monster);
            }
        }
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

    // ���� ����
    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[MonsterManager] ���� ����Ʈ�� �������");
            return transform.position; // fallback
        }

        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].position;
    }
}
