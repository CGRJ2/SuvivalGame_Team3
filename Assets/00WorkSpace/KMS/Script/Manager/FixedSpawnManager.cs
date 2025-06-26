using System.Collections.Generic;
using UnityEngine;

public class FixedSpawnManager : MonoBehaviour
{
    [SerializeField] private WaveData fixedSpawnData;  // ������ ��� WaveData
    [SerializeField] private CatSpawnData catSpawnData;// ����̰� ��� CatSpawnData

    private Dictionary<string, BaseMonster> fixedMonsters = new();

    public void RespawnFixedMonsters()
    {
        foreach (var info in fixedSpawnData.fixedSpawns)
        {
            // �̹� �����ϸ� ���� �� �ٽ� ���� (�����)
            if (fixedMonsters.TryGetValue(info.monsterData.monsterName, out var existing))
            {
                if (existing != null)
                    Destroy(existing.gameObject);
            }

            var monster = MonsterFactory.Instance.SpawnMonster(info.monsterData, info.spawnPoint);
            if (monster != null)
            {
                monster.SetTarget(FindTargetByType(info.monsterData.TargetType));
                fixedMonsters[info.monsterData.monsterName] = monster;
            }
        }

        Debug.Log("[FixedSpawnManager] ���� ���� ������ �Ϸ�");
    }

    private Transform FindTargetByType(MonsterTargetType type)
    {
        return type switch
        {
            MonsterTargetType.Player => GameObject.FindWithTag("Player")?.transform,
            MonsterTargetType.Ally => GameObject.FindWithTag("Ally")?.transform,
            _ => null
        };
    }

    public void SpawnCat(int currentDay)
    {
        Vector3 spawnPos = GetSpawnPositionForDay(catSpawnData.catSpawnInfo, currentDay);
        var cat = MonsterFactory.Instance.SpawnMonster(catSpawnData.catSpawnInfo.monsterData, spawnPos);

        if (cat != null)
            cat.SetTarget(FindTargetByType(catSpawnData.catSpawnInfo.monsterData.TargetType));
    }

    private Vector3 GetSpawnPositionForDay(VariableSpawnInfo info, int day)
    {
        foreach (var entry in info.daySpawnList)
        {
            if (entry.day == day)
                return entry.spawnPosition;
        }

        // �⺻�� Ȥ�� ���� ����
        return info.daySpawnList.Count > 0
            ? info.daySpawnList[Random.Range(0, info.daySpawnList.Count)].spawnPosition
            : Vector3.zero;
    }
}