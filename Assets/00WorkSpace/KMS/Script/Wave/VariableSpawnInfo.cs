using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VariableSpawnInfo
{
    public BaseMonsterData monsterData;
    public List<DaySpawnInfo> daySpawnList;
}

[System.Serializable]
public class DaySpawnInfo
{
    public int day;
    public Vector3 spawnPosition;
}