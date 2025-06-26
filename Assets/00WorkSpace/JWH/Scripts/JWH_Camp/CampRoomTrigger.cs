using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampRoomTrigger : MonoBehaviour
{
    [SerializeField] private string roomId;
    [SerializeField] private CampRoomChecker roomChecker; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        roomChecker.RoomRegister(roomId);
        Debug.Log($"RoomTrigger : {roomId}");
    }
}
