using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CampRoomChecker : MonoBehaviour
{
    private HashSet<string> openedRooms = new HashSet<string>();

    public void RoomRegister(string roomId)
    {
        if (openedRooms.Add(roomId))
        {
            Debug.Log($"RoomTracker ¹æ ¿ÀÇÂµÊ: {roomId}");
        }
    }

    public List<string> GetOpenedRoomIds() => openedRooms.ToList();
}
