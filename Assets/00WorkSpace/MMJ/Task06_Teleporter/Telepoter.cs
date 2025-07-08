using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepoter : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint; // �ڷ���Ʈ ������
    [SerializeField] private bool teleportImmediately = true; // ��� �ڷ���Ʈ ����
    [SerializeField] private float teleportDelay = 0f; // �ڷ���Ʈ ���� �ð� (��)
    [SerializeField] private bool resetVelocity = true; // �ڷ���Ʈ �� �ӵ� �ʱ�ȭ ����

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾����� Ȯ��
        if (other.CompareTag("Player"))
        {
            if (teleportImmediately)
            {
                TeleportPlayer(other.gameObject);
            }
            else
            {
                // ���� �ڷ���Ʈ
                StartCoroutine(DelayedTeleport(other.gameObject));
            }
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        if (destinationPoint != null)
        {
            // �÷��̾��� Rigidbody ��������
            Rigidbody rb = player.GetComponent<Rigidbody>();

            // �ӵ� �ʱ�ȭ (�ɼ�)
            if (rb != null && resetVelocity)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // �÷��̾� ��ġ ����
            player.transform.position = destinationPoint.position;

            // ���⵵ ���߰� �ʹٸ� (�ɼ�)
            // player.transform.rotation = destinationPoint.rotation;

            Debug.Log("�÷��̾ �ڷ���Ʈ�Ǿ����ϴ�!");
        }
        else
        {
            Debug.LogError("�ڷ���Ʈ �������� �������� �ʾҽ��ϴ�!");
        }
    }

    private IEnumerator DelayedTeleport(GameObject player)
    {
        yield return new WaitForSeconds(teleportDelay);
        TeleportPlayer(player);
    }
}
