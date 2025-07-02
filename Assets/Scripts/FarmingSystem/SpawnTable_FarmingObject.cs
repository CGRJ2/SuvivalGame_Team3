using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FOSpawnTable", menuName = "New SpawnTable/FarmingObjectTable")]
public class SpawnTable_FarmingObject : SpawnTable
{
    protected void OnEnable()
    {
        GameObject[] prefabs;
        switch (stageKey)
        {
            case StageKey.All:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/FarmingObjects/99Generic");
                break;

            case StageKey.�Ž�:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/FarmingObjects/00LivingRoom");
                break;

            case StageKey.����:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/FarmingObjects/01Library");
                break;

            case StageKey.�ʹ�:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/FarmingObjects/02DressRoom");
                break;

            case StageKey.�ȹ�:
                prefabs = Resources.LoadAll<GameObject>("Prefabs/FarmingObjects/03MasterBedroom");
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
