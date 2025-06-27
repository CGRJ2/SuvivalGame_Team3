using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JWH_Test/TestRecovery/TestFullRecoveryItem")]
public class TestFullRecoveryItem : ScriptableObject
{
    public void Use(PlayerStatus player)
    {
        player.WillPower.Value = 100;
        player.Battery.Value = 100;

        foreach (var part in player.GetBodyPartsList())
        {
            part.Repair(9999); 
        }

        Debug.Log("FullRecoveryItem");
    }
}
