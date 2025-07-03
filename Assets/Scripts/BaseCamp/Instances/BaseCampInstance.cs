using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampInstance : CampInstance
{
    void Start()
    {
        BaseCampManager.Instance.baseCampInstance = this;
        BaseCampManager.Instance.baseCampData.baseCampTransform = transform;
    }

    public Transform GetRespawnPointTransform()
    {
        return respawnPoint;
    }
}
