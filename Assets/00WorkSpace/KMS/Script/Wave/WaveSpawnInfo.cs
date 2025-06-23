using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSpawnInfo
{
    public BaseMonsterData monsterData;
    public BaseMonster monsterPrefab;
    public int count;
    public float spawnDelay;
    public Vector3 spawnPosition;

}