using System.Collections;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [Header("���� �Ŀ�")]
    [SerializeField] private float jumpForce = 10f; // ���� ��
    [Header("���� �� ������ ���ð�")]
    [SerializeField] private float waitTimeToActive = 2f; // ���� ��
    [Header("���� �ӵ� �ʱ�ȭ ����")]
    [SerializeField] private bool resetVerticalVelocity = true; // ���� �ӵ� �ʱ�ȭ ����
    [Header("����ӵ� ���� ����")]
    [SerializeField] private bool preserveHorizontalVelocity = true; // ���� �ӵ� ���� ����


    [SerializeField] Animator animator;


    bool isActive = false;


    private void Awake()
    {
        isActive = false;
        StartCoroutine(WaitForActive());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator WaitForActive()
    {
        yield return new WaitForSeconds(waitTimeToActive);
        isActive = true;
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(waitTimeToActive);
        Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        // �÷��̾����� Ȯ��
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            LaunchPlayer(other.gameObject);
        }
    }

    private void LaunchPlayer(GameObject player)
    {
        if (!isActive) return;
        isActive = false;
        // �÷��̾��� Rigidbody ��������
        Rigidbody rb = player.GetComponent<Rigidbody>();

        animator.SetTrigger("Jump");

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

            Debug.Log("�÷��̾ �����뿡�� �߻�Ǿ����ϴ�!");

            StartCoroutine(WaitForDestroy());
        }
    }

}
