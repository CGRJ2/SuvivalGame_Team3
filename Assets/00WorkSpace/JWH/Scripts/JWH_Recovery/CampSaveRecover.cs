using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInteract : MonoBehaviour, IInteractable
{
    public FakeSave fakeSave;

    public void Interact()
    {
        var player = FindObjectOfType<PlayerStatus>();
        var recovery = GetComponent<CampRecovery>();
        if (recovery != null)
        {
            recovery.CampRecover(player);
        }

        if (fakeSave != null)
        {
            fakeSave.TryNotSave();
        }

        if (fakeSave != null)
        {
            fakeSave.NotLoad();
        }

    }

    public void SetInteractableEnable()
    {
        Debug.Log("세이브 & 회복");
    }
}