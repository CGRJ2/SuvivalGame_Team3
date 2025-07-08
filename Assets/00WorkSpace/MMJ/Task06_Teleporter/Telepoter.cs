using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepoter : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint; // 텔레포트 목적지
    [SerializeField] private bool teleportImmediately = true; // 즉시 텔레포트 여부
    [SerializeField] private float teleportDelay = 0f; // 텔레포트 지연 시간 (초)
    [SerializeField] private bool resetVelocity = true; // 텔레포트 후 속도 초기화 여부

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            if (teleportImmediately)
            {
                TeleportPlayer(other.gameObject);
            }
            else
            {
                // 지연 텔레포트
                StartCoroutine(DelayedTeleport(other.gameObject));
            }
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        if (destinationPoint != null)
        {
            // 플레이어의 Rigidbody 가져오기
            Rigidbody rb = player.GetComponent<Rigidbody>();

            // 속도 초기화 (옵션)
            if (rb != null && resetVelocity)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // 플레이어 위치 변경
            player.transform.position = destinationPoint.position;

            // 방향도 맞추고 싶다면 (옵션)
            // player.transform.rotation = destinationPoint.rotation;

            Debug.Log("플레이어가 텔레포트되었습니다!");
        }
        else
        {
            Debug.LogError("텔레포트 목적지가 설정되지 않았습니다!");
        }
    }

    private IEnumerator DelayedTeleport(GameObject player)
    {
        yield return new WaitForSeconds(teleportDelay);
        TeleportPlayer(player);
    }
}
