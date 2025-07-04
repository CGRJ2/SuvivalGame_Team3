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
            UIController.Instance.ShowNPCTalk("어서오너라 메이플은 프로젝트가 끝난 다음에 하거라.",2f);
        }
    }
}
