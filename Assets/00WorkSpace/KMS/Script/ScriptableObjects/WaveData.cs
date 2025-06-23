using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/WaveData")]
public class WaveData : ScriptableObject
{
    public string waveName;
    public float spawnDelay;
    public int totalCount;
    public List<WaveSpawnInfo> spawnInfos;
}