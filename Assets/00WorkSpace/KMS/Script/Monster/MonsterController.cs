using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseMonster))]
public class MonsterController : MonoBehaviour
{
    private BaseMonster monster;

    [Header("Movement")]
    [SerializeField] private float turnSpeed = 5f;

    private void Awake()
    {
        monster = GetComponent<BaseMonster>();
    }

    private void Update()
    {
        if (monster == null || monster.IsDead) return;

        Transform target = monster.GetTarget();
        if (target == null) return;

        Vector3 toTarget = (target.position - transform.position);
        toTarget.y = 0f; // 수평 회전만

        if (toTarget.magnitude < 0.1f)
            return;

        // 이동
        Vector3 direction = toTarget.normalized;
        monster.Agent.SetDestination(direction);

        // 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}

