using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Payload payload; // 비행기 연결
    public Transform respawnPoint; // 리스폰 지점
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
    // 플레이어가 죽었을 때
    public void Die()
    {
        if (payload != null)
        {
            payload.ResetToStart();
        }

        if (playerCollider != null) playerCollider.enabled = false;
        foreach (var r in renderers) r.enabled = false;
        // 플레이어 리스폰
        StartCoroutine(Respawn());
    }

    System.Collections.IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f); // 리스폰 시간

        transform.position = respawnPoint.position;
        curHP = maxHP;

        if (playerCollider != null) playerCollider.enabled = true;
        foreach (var r in renderers) r.enabled = true;
    }
}
