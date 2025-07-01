using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampInstance : CampInstance
{
    void Start()
    {
        BaseCampManager.Instance.baseCampInstance = this;
    }

    public Transform GetRespawnPointTransform()
    {
        return respawnPoint;
    }
}
