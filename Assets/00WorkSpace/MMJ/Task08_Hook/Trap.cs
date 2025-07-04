using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IDamagable
{
    [Header("Trap Settings")]
    [SerializeField] private int maxHealth = 2;          // Ʈ���� ������
    [SerializeField] private float respawnTime = 180f;   // ��Ȱ��ȭ���� �ð�
    [SerializeField] private int damageAmount = 10;      // �÷��̾�� �ִ� ������
    [SerializeField] private float knockbackForce = 5f;  // �÷��̾� �˹� ��

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


    private void ApplyKnockback(GameObject player) // ������ �ִ� �˹��� ������ �˹��� ��� �����Ǿ��ִ��� ����
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            // ���� �÷��̾� �ӵ� �ʱ�ȭ (���� ������ ���)
            playerRb.velocity = Vector3.zero;

            // �������� �÷��̾� �������� �˹� (�� ���� �� ����)
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            knockbackDirection.y = 0.5f;

            // �� ���� �� ���� (�������� 2-3��)
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
