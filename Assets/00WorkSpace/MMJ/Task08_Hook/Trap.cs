using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IDamagable
{
    [Header("Trap Settings")]
    [SerializeField] private int maxHealth = 2;          // 트랩의 내구도
    [SerializeField] private float respawnTime = 180f;   // 재활성화까지 시간
    [SerializeField] private int damageAmount = 10;      // 플레이어에게 주는 데미지
    [SerializeField] private float knockbackForce = 5f;  // 플레이어 넉백 힘

    private int currentHealth;
    private Collider trapCollider;
    private Renderer trapRenderer;

    private void Awake()
    {
        currentHealth = maxHealth;
        trapCollider = GetComponent<Collider>();
        trapRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trap Trigger Enter");

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageAmount, transform);

               
            }
        }
    }

    public void TakeDamage(int amount, Transform attacker)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            StartCoroutine(DeactivateTrap());
        }
    }


    private void ApplyKnockback(GameObject player) // 문제가 있는 넉백기능 몬스터의 넉백이 어떻게 구현되어있는지 보자
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            // 현재 플레이어 속도 초기화 (기존 움직임 상쇄)
            playerRb.velocity = Vector3.zero;

            // 함정에서 플레이어 방향으로 넉백 (더 강한 힘 적용)
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            knockbackDirection.y = 0.5f;

            // 더 강한 힘 적용 (기존보다 2-3배)
            playerRb.AddForce(knockbackDirection * knockbackForce * 2.5f, ForceMode.Impulse);
        }
    }

    private IEnumerator DeactivateTrap()
    {
        trapCollider.enabled = false;
        trapRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        currentHealth = maxHealth;
        trapCollider.enabled = true;
        trapRenderer.enabled = true;
    }
}
