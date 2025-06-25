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
            Debug.Log("ķ�� ���׷��̵�");
        }
    }

    public void Interact()
    {
        UpgradeCamp();
    }

    public void SetInteractableEnable()
    {
        Debug.Log("ķ�� ��ȣ�ۿ�");
        
    }
}
