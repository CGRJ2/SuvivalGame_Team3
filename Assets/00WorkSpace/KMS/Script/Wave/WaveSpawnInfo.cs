using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSpawnInfo
{
    public BaseMonsterData monsterData;
    public int count;
    public float spawnDelay;
    public Vector3 spawnPosition;
}