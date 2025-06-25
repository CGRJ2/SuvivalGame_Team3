using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp : MonoBehaviour, IInteractable
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

    public void Interact()
    {
        UpgradeCamp();
    }

    public void SetInteractableEnable()
    {
        Debug.Log("캠프 상호작용");
        
    }
}
