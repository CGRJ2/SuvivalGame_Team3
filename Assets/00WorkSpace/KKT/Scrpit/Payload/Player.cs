using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Payload payload; // ����� ����
    public Transform respawnPoint; // ������ ����
    public int maxHP=100;
    public int curHP;

    public Collider playerCollider;
    public Renderer[] renderers;

    private void Start()
    {
        curHP = maxHP;
        if (GameState.lastPlayerHP != 0 || GameState.lastPlayerPos != Vector3.zero)
        {
            curHP = GameState.lastPlayerHP;
            transform.position = GameState.lastPlayerPos;
        }
        Debug.Log($"{GameState.isSave}");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(50);
        }
    }
    public void TakeDamage(int amount)
    {
        curHP -= amount;
        if (curHP <= 0) Die();
    }
    // �÷��̾ �׾��� ��
    public void Die()
    {
        if (payload != null)
        {
            payload.ResetToStart();
        }

        if (playerCollider != null) playerCollider.enabled = false;
        foreach (var r in renderers) r.enabled = false;
        // �÷��̾� ������
        StartCoroutine(Respawn());
    }

    System.Collections.IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f); // ������ �ð�

        transform.position = respawnPoint.position;
        curHP = maxHP;

        if (playerCollider != null) playerCollider.enabled = true;
        foreach (var r in renderers) r.enabled = true;
    }
}
