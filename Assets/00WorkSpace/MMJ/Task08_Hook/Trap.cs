using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IDamagable
{
    [Header("Trap Settings")]
    [SerializeField] private int maxHealth = 2;          // 트랩의 내구도 (플레이어 공격 횟수)
    [SerializeField] private float respawnTime = 180f;   // 재활성화까지 시간 (3분 = 180초)
    [SerializeField] private int damageAmount = 10;      // 플레이어에게 주는 데미지
    [SerializeField] private float knockbackForce = 0.8f;  // 플레이어 넉백 힘

    // 실제 함정의 시각적/물리적 부분을 담을 자식 오브젝트
    [Header("Child Object for Visuals & Collision")]
    [SerializeField] private GameObject trapVisualAndCollisionObject;

    private int currentHealth;
    private bool isActive = true; // 함정 활성화 상태 (로직 상의 상태)

    private void Awake()
    {
        currentHealth = maxHealth;
        // 시작 시 자식 오브젝트 활성화 (초기 상태)
        SetTrapState(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 함정이 활성화 상태일 때만 데미지 판정
        if (!isActive) return; // 로직 상의 상태 확인

        Debug.Log("Trap Trigger Enter");

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>(); 
            if (player != null)
            {
                Debug.Log("플레이어가 공격받았습니다");
                player.TakeDamage(damageAmount, transform);
                ApplyKnockback(other.gameObject);
            }
        }
    }

    public void TakeDamage(float amount, Transform attacker) // 데미지 받기와 상태 체크 후 비활성
    {

        Debug.Log($"함정이 공격을 받았습니다. 남은 체력: {currentHealth}");
        if (!isActive) return;

        currentHealth -= 1; // 공격력과 무관하게 항상 1씩 감소
        Debug.Log($"함정이 공격을 받았습니다. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            StartCoroutine(DeactivateAndRespawn());
        }
    }

private void ApplyKnockback(GameObject player)
    {
        // 플레이어 컨트롤러와 리지드바디 가져오기
        PlayerController playerController = player.GetComponent<PlayerController>();
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        if (playerController != null && playerRb != null)
        {
            // 플레이어 이동 일시적으로 비활성화
            playerController.Status.isControllLocked = true;

            // 현재 플레이어 속도 초기화
            playerRb.velocity = Vector3.zero;

            // 플레이어 뒷 방향으로 넉백
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            knockbackDirection.x *= 1.8f;
            knockbackDirection.z *= 1.8f;
            knockbackDirection.y = 1.0f; // 위로도 약간 띄우기

            // 뒤로 밀어내기
            playerRb.AddForce(knockbackDirection * knockbackForce * 3f, ForceMode.Impulse);

            // 일정 시간 후 플레이어 이동 다시 활성화
            StartCoroutine(EnablePlayerMovementAfterDelay(playerController, 0.5f));
        }
    }

    // 일정 시간 후 플레이어 이동 다시 활성화하는 코루틴
    private IEnumerator EnablePlayerMovementAfterDelay(PlayerController playerController, float delay)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 플레이어 이동 다시 활성화
        playerController.Status.isControllLocked = false;
    }

    private IEnumerator DeactivateAndRespawn()
    {
        Debug.Log("함정 파괴! 비활성화 및 재활성화 대기 시작.");
        SetTrapState(false); // 함정 비활성화 상태로 전환

        yield return new WaitForSeconds(respawnTime); // 재활성화 시간 대기

        Debug.Log("함정 재활성화.");
        SetTrapState(true); // 함정 활성화 상태로 전환
        currentHealth = maxHealth; // 체력 초기화
    }

    private void SetTrapState(bool active) // 함정 자식 오브젝트를 활성화/비활성화
    {
        isActive = active; // 로직 상의 상태 업데이트

        if (trapVisualAndCollisionObject != null)
        {
            trapVisualAndCollisionObject.SetActive(active);
        }
    }
}