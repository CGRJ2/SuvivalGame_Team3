using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f; // 점프 힘
    [SerializeField] private bool resetVerticalVelocity = true; // 수직 속도 초기화 여부
    [SerializeField] private bool preserveHorizontalVelocity = true; // 수평 속도 유지 여부

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            LaunchPlayer(other.gameObject);
        }
    }

    private void LaunchPlayer(GameObject player)
    {
        // 플레이어의 Rigidbody 가져오기
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // 현재 속도 저장
            Vector3 currentVelocity = rb.velocity;

            // 수직 속도 초기화 (옵션)
            if (resetVerticalVelocity)
            {
                currentVelocity.y = 0f;
            }

            // 수평 속도 처리
            if (!preserveHorizontalVelocity)
            {
                currentVelocity.x = 0f;
                currentVelocity.z = 0f;
            }

            // 새 속도 설정
            rb.velocity = currentVelocity;

            // 위쪽 방향으로 힘 가하기
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Debug.Log("플레이어가 점프대에서 발사되었습니다!");
        }
    }
}
