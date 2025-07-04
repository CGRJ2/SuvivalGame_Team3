using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Payload : MonoBehaviour
{
    public Transform[] checkPoints; // ��������
    public float moveSpeed; // ȭ�� �̵� �ӵ�
    public float playerDetectRadius; // �÷��̾� �ν� ����
    public GameObject player; // �÷��̾�

    private int curCheckPointIndex = 0;
    private bool isMoving = false; // �̵� ���� Ȯ��
    private bool atEnd = false; // ���� ���� Ȯ��
    private bool isUnlocked = false; // ���� Ȯ��

    private void Start()
    {
        transform.position = checkPoints[0].position;
    }
    private void Update()
    {
        if (atEnd) return;
        if (player == null) return;

        // �÷��̾ �ν� ���� �ȿ� �ִ���
        float dist = Vector3.Distance(player.transform.position, transform.position);
        bool isPlayerNear = dist <= playerDetectRadius;

        // ������ �� �߰�, �̵� ���� ��, �÷��̾ ���� �ȿ� ����, ��ȣ �ۿ��� ���� ���
        if (!isUnlocked && !isMoving && isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("���� ���ؼ� ��ȣ�ۿ� �Ұ���");
        }

        // ������ �߰�, �̵� ���� ��, �÷��̾ ���� �ȿ� ����, ��ȣ �ۿ��� ���� ���
        if (isUnlocked &&!isMoving && isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("���� �Ϸ�! ����� �̵�");
            isMoving = true;
        }

        // �̵� ���� ��, �÷��̾ ���� �� ����
        if (isMoving && isPlayerNear)
        {
            Vector3 target = checkPoints[curCheckPointIndex].position;
            Vector3 dir = (target - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            // ���� üũ
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                transform.position = target;
                curCheckPointIndex++;
                if (curCheckPointIndex >= checkPoints.Length)
                {
                    atEnd = true;
                    isMoving = false;
                    OnPayloadArrived();
                }
            }
        }
    }
    // �÷��̾� ����� �����
    public void ResetToStart()
    {
        atEnd = false;
        curCheckPointIndex = 0;
        isMoving = false;
        transform.position = checkPoints[0].position;
    }
    // ���� ������ ����
    private void OnPayloadArrived()
    {
        Debug.Log("ȭ�� ����");
        SceneManager.LoadScene("EmptyTestScene");
    }
    // ���� ���� �� ���� ����
    public void UnlockPayload()
    {
        isUnlocked = true;
    }
}
