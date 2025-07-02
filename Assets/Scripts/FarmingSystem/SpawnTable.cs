using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTable : ScriptableObject
{
    public int level;
    public StageKey stageKey;
    public List<SpawnObjectInfo> objectList;

}

[System.Serializable]
public class SpawnObjectInfo
{
    public GameObject spawnObject;
    public int objectWeight;
}


