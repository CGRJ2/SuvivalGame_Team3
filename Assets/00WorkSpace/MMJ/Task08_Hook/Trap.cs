using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IDamagable
{
    [Header("Trap Settings")]
    [SerializeField] private int maxHealth = 2;          // Ʈ���� ������ (�÷��̾� ���� Ƚ��)
    [SerializeField] private float respawnTime = 180f;   // ��Ȱ��ȭ���� �ð� (3�� = 180��)
    [SerializeField] private int damageAmount = 10;      // �÷��̾�� �ִ� ������
    [SerializeField] private float knockbackForce = 0.8f;  // �÷��̾� �˹� ��

    // ���� ������ �ð���/������ �κ��� ���� �ڽ� ������Ʈ
    [Header("Child Object for Visuals & Collision")]
    [SerializeField] private GameObject trapVisualAndCollisionObject;

    private int currentHealth;
    private bool isActive = true; // ���� Ȱ��ȭ ���� (���� ���� ����)

    private void Awake()
    {
        currentHealth = maxHealth;
        // ���� �� �ڽ� ������Ʈ Ȱ��ȭ (�ʱ� ����)
        SetTrapState(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������ Ȱ��ȭ ������ ���� ������ ����
        if (!isActive) return; // ���� ���� ���� Ȯ��

        Debug.Log("Trap Trigger Enter");

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>(); 
            if (player != null)
            {
                Debug.Log("�÷��̾ ���ݹ޾ҽ��ϴ�");
                player.TakeDamage(damageAmount, transform);
                ApplyKnockback(other.gameObject);
            }
        }
    }

    public void TakeDamage(float amount, Transform attacker) // ������ �ޱ�� ���� üũ �� ��Ȱ��
    {

        Debug.Log($"������ ������ �޾ҽ��ϴ�. ���� ü��: {currentHealth}");
        if (!isActive) return;

        currentHealth -= 1; // ���ݷ°� �����ϰ� �׻� 1�� ����
        Debug.Log($"������ ������ �޾ҽ��ϴ�. ���� ü��: {currentHealth}");

        if (currentHealth <= 0)
        {
            StartCoroutine(DeactivateAndRespawn());
        }
    }

private void ApplyKnockback(GameObject player)
    {
        // �÷��̾� ��Ʈ�ѷ��� ������ٵ� ��������
        PlayerController playerController = player.GetComponent<PlayerController>();
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        if (playerController != null && playerRb != null)
        {
            // �÷��̾� �̵� �Ͻ������� ��Ȱ��ȭ
            playerController.Status.isControllLocked = true;

            // ���� �÷��̾� �ӵ� �ʱ�ȭ
            playerRb.velocity = Vector3.zero;

            // �÷��̾� �� �������� �˹�
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            knockbackDirection.x *= 1.8f;
            knockbackDirection.z *= 1.8f;
            knockbackDirection.y = 1.0f; // ���ε� �ణ ����

            // �ڷ� �о��
            playerRb.AddForce(knockbackDirection * knockbackForce * 3f, ForceMode.Impulse);

            // ���� �ð� �� �÷��̾� �̵� �ٽ� Ȱ��ȭ
            StartCoroutine(EnablePlayerMovementAfterDelay(playerController, 0.5f));
        }
    }

    // ���� �ð� �� �÷��̾� �̵� �ٽ� Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator EnablePlayerMovementAfterDelay(PlayerController playerController, float delay)
    {
        // ������ �ð���ŭ ���
        yield return new WaitForSeconds(delay);

        // �÷��̾� �̵� �ٽ� Ȱ��ȭ
        playerController.Status.isControllLocked = false;
    }

    private IEnumerator DeactivateAndRespawn()
    {
        Debug.Log("���� �ı�! ��Ȱ��ȭ �� ��Ȱ��ȭ ��� ����.");
        SetTrapState(false); // ���� ��Ȱ��ȭ ���·� ��ȯ

        yield return new WaitForSeconds(respawnTime); // ��Ȱ��ȭ �ð� ���

        Debug.Log("���� ��Ȱ��ȭ.");
        SetTrapState(true); // ���� Ȱ��ȭ ���·� ��ȯ
        currentHealth = maxHealth; // ü�� �ʱ�ȭ
    }

    private void SetTrapState(bool active) // ���� �ڽ� ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ
    {
        isActive = active; // ���� ���� ���� ������Ʈ

        if (trapVisualAndCollisionObject != null)
        {
            trapVisualAndCollisionObject.SetActive(active);
        }
    }
}