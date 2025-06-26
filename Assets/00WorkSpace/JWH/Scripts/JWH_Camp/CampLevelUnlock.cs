using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp : MonoBehaviour, IInteractable
{
    public CampLevel camplevel;
    public CampUnlock campunlock;
    //public CampRoomChecker roomChecker;

    public void UpgradeCamp()
    {
        if (camplevel.TryLevelUp())
        {
            //var openedRooms = roomChecker.GetOpenedRoomIds();
            campunlock.UnlockItems(camplevel.CurrentLevel);//openedRooms
            Debug.Log("ķ�� ���׷��̵�");
        }
    }

    public void Interact()
    {
        UpgradeCamp();
    }

    public void SetInteractableEnable()
    {
        Debug.Log("ķ�� ���ͷ�");
    }
}
