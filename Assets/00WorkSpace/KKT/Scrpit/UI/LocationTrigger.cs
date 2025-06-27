using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName;  // Inspector에서 지역 이름 지정
    public float messageTime = 2f;

    void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        if (LocationManager.CurrentLocation != locationName )
        {
            LocationManager.CurrentLocation = locationName;
            UIController.Instance.ShowLocationNotification($"현재 위치 : {locationName}", messageTime);

            UIController.Instance.UpdateCurrentLocation(locationName);
        }
    }
}
