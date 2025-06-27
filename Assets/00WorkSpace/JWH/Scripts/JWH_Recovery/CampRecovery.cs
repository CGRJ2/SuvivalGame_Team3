using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampRecovery : MonoBehaviour
{
    public void CampRecover(PlayerStatus player)
    {
        
        player.WillPower.Value = 100;
        player.Battery.Value = 100;

        foreach (var part in player.GetBodyPartsList())
        {
            part.Repair(9999);
        }

        //foreach (var part in player.Status.GetBodyPartsList())
        //{
        //    part.Init(); // ����ȸ��???
        //}

        Debug.Log("BaseCampRecovery �÷��̾�ȸ��");

    }
    
}