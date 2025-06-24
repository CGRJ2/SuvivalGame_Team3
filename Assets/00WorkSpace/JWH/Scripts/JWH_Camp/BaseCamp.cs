using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp : MonoBehaviour
{
    public CampLevel camplevel;
    public CampUnlock campunlock;

    public void UpgradeCamp()
    {
        if (camplevel.TryLevelUp())
        {
            campunlock.UnlockItems(camplevel.CurrentLevel);
            Debug.Log("캠프 업그레이드");
        }
    }
}
