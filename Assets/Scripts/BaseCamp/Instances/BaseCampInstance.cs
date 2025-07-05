using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampInstance : CampInstance
{
    void Start()
    {
        BaseCampManager.Instance.baseCampInstance = this;
        BaseCampManager.Instance.baseCampData.baseCampPosition = transform.position;
        BaseCampManager.Instance.baseCampData.baseCampRotation = transform.rotation;
    }

    public Transform GetRespawnPointTransform()
    {
        return respawnPoint;
    }
}
