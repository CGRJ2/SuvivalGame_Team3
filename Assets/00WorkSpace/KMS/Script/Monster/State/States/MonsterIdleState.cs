using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : IMonsterState
{
    private BaseMonster monster;
    private float timer;
    private float direction = 1f;
    private float lookDuration = 2f;
    private float rotateSpeed = 30f;
    private Quaternion baseRotation;
    private float lookTimer = 0f;
    private float lookInterval;
    private Quaternion targetRotation;
    private Quaternion baseRotationTarget;
    private float baseRotateSmooth = 2f; // 기준방향 회전 속도

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        lookTimer = 0f;
        lookInterval = 2.5f + Random.Range(0f, 1.5f);

        float startY = monster.transform.rotation.eulerAngles.y;
        baseRotation = Quaternion.Euler(0, startY, 0);
        baseRotationTarget = baseRotation;

        lookDuration = Random.Range(1.5f, 3f);

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{monster.name}] 상태: Idle 진입 (좌우 회전)");
    }


    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        timer += Time.deltaTime;
        lookTimer += Time.deltaTime;

        // 2~4초마다 기준 방향 목표값을 변경
        if (lookTimer >= lookInterval)
        {
            SetRandomBaseRotationTarget();
            lookTimer = 0f;
            lookInterval = 2.5f + Random.Range(0f, 1.5f);
        }

        baseRotation = Quaternion.Slerp(baseRotation, baseRotationTarget, Time.deltaTime * baseRotateSmooth);

        float angleOffset = Mathf.Sin(timer * Mathf.PI / lookDuration) * 20f;
        Quaternion targetRotation = baseRotation * Quaternion.Euler(0f, angleOffset, 0f);

        monster.transform.rotation = Quaternion.Slerp(
            monster.transform.rotation,
            targetRotation,
            Time.deltaTime * 3f
        );

        if (timer >= lookDuration * 2f)
        {
            timer = 0f;
            lookDuration = Random.Range(1.5f, 3f);
            Debug.Log($"[{monster.name}] 좌우 회전 루프 반복");
        }
    }

    private void SetRandomBaseRotationTarget()
    {
        float randomY = Random.Range(0f, 360f);
        baseRotationTarget = Quaternion.Euler(0, randomY, 0);
        Debug.Log($"[Monster] 기준 방향 목표 전환: {randomY}도");
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: Idle 종료 (회전 초기화)");
    }
}
