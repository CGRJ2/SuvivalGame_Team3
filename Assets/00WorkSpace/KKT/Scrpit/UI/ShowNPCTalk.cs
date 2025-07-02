using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNPCTalk : MonoBehaviour
{
    private bool canInteract = false;
    public UIController UIController;

    //public void Update()
    //{
    //    if (canInteract)
    //    {
    //        UIController.Instance.ShowNPCTalk(true);
    //    }
    //    else
    //    {
    //        UIController.Instance.ShowNPCTalk(false);
    //    }
    //}
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            UIController.Instance.ShowNPCTalk(true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            UIController.Instance.ShowNPCTalk(false);
        }
    }
}
