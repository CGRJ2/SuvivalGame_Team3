using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnTestUI : MonoBehaviour
{
    public void SpawnWaveTest()
    {
        if (MonsterManager.Instance != null)
            MonsterManager.Instance.StartWave(0);
    }
}
