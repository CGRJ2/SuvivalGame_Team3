using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f; // ���� ��
    [SerializeField] private bool resetVerticalVelocity = true; // ���� �ӵ� �ʱ�ȭ ����
    [SerializeField] private bool preserveHorizontalVelocity = true; // ���� �ӵ� ���� ����
    [SerializeField] private ParticleSystem jumpEffect; // ���� ����Ʈ (�ɼ�)
    [SerializeField] private AudioClip jumpSound; // ���� ���� (�ɼ�)

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾����� Ȯ��
        if (other.CompareTag("Player"))
        {
            LaunchPlayer(other.gameObject);
        }
    }

    private void LaunchPlayer(GameObject player)
    {
        // �÷��̾��� Rigidbody ��������
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // ���� �ӵ� ����
            Vector3 currentVelocity = rb.velocity;

            // ���� �ӵ� �ʱ�ȭ (�ɼ�)
            if (resetVerticalVelocity)
            {
                currentVelocity.y = 0f;
            }

            // ���� �ӵ� ó��
            if (!preserveHorizontalVelocity)
            {
                currentVelocity.x = 0f;
                currentVelocity.z = 0f;
            }

            // �� �ӵ� ����
            rb.velocity = currentVelocity;

            // ���� �������� �� ���ϱ�
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // ����Ʈ ��� (�ɼ�)
            if (jumpEffect != null)
            {
                jumpEffect.Play();
            }

            // ���� ��� (�ɼ�)
            if (jumpSound != null && GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().PlayOneShot(jumpSound);
            }

            Debug.Log("�÷��̾ �����뿡�� �߻�Ǿ����ϴ�!");
        }
    }
}
