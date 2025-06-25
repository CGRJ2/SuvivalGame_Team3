using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName;  // Inspector���� ���� �̸� ����
    public float messageTime = 2f;

    void OnTriggerEnter(Collider other)
    {
        if (LocationManager.CurrentLocation == locationName) return;

        LocationManager.CurrentLocation = locationName;

        UIController.Instance.ShowLocationNotification($"{locationName}�� �����߽��ϴ�!", messageTime);

        Debug.Log(LocationManager.CurrentLocation);
    }
}
