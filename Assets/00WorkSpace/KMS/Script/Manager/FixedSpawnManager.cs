using System.Collections.Generic;
using UnityEngine;

public class FixedSpawnManager : MonoBehaviour
{
    [SerializeField] private WaveData fixedSpawnData;  // 주인이 담긴 WaveData
    [SerializeField] private CatSpawnData catSpawnData;// 고양이가 담긴 CatSpawnData

    private Dictionary<string, BaseMonster> fixedMonsters = new();

    public void RespawnFixedMonsters()
    {
        foreach (var info in fixedSpawnData.fixedSpawns)
        {
            // 이미 존재하면 제거 후 다시 생성 (재시작)
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

        Debug.Log("[FixedSpawnManager] 고정 몬스터 리스폰 완료");
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

        // 기본값 혹은 랜덤 지정
        return info.daySpawnList.Count > 0
            ? info.daySpawnList[Random.Range(0, info.daySpawnList.Count)].spawnPosition
            : Vector3.zero;
    }
}