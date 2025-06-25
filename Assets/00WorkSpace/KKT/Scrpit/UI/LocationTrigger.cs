using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName;  // Inspector에서 지역 이름 지정
    public float messageTime = 2f;

    void OnTriggerEnter(Collider other)
    {
        if (LocationManager.CurrentLocation == locationName) return;

        LocationManager.CurrentLocation = locationName;

        UIController.Instance.ShowLocationNotification($"{locationName}에 입장했습니다!", messageTime);

        Debug.Log(LocationManager.CurrentLocation);
    }
}
