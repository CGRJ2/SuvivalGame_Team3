using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MonsterSpawnTable", menuName = "New SpawnTable/MonsterTable")]
public class SpawnTable_Monster : SpawnTable
{
    protected void OnEnable()
    {
        GameObject[] prefabs;
        switch (stageKey)
        {
            case StageKey.All:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/Monsters/99Generic");
                break;

            case StageKey.거실:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/Monsters/00LivingRoom");
                break;

            case StageKey.서재:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/Monsters/01Library");
                break;

            case StageKey.옷방:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/Monsters/02DressRoom");
                break;

            case StageKey.안방:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/Monsters/03MasterBedroom");
                break;

            default: prefabs = new GameObject[0]; break;
        }


        for (int i = 0; i < prefabs.Length; i++)
        {
            if (objectList == null) objectList = new List<SpawnObjectInfo>();
            if (objectList.Count - 1 < i)
            {
                objectList.Add(new SpawnObjectInfo());
            }

            objectList[i].spawnObject = prefabs[i];
        }
    }
}
