using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName;  // Inspector���� ���� �̸� ����
    public float messageTime = 2f;

    void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        if (LocationManager.CurrentLocation != locationName )
        {
            LocationManager.CurrentLocation = locationName;
            UIController.Instance.ShowLocationNotification($"���� ��ġ : {locationName}", messageTime);

            UIController.Instance.UpdateCurrentLocation(locationName);
        }
    }
}
