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

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        direction = 1f;
        lookDuration = Random.Range(1.5f, 3f);

        baseRotation = monster.transform.rotation;

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{monster.name}] 상태: Idle 진입 (좌우 회전)");
    }

    public void Execute()
    {
        monster.transform.GetChild(0).Rotate(0f, 30f * Time.deltaTime, 0f);

        if (monster == null || monster.IsDead) return;

        // 플레이어 감지
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f);
        }

        timer += Time.deltaTime;

        // 회전: 기준에서 좌우로 +-20도 사이로 왕복 회전
        float angleOffset = Mathf.Sin(timer * Mathf.PI / lookDuration) * 20f;
        Quaternion targetRotation = baseRotation * Quaternion.Euler(0f, angleOffset, 0f);
        monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, targetRotation, Time.deltaTime * 3f);

        // 일정 시간 뒤 상태 재전이
        if (timer >= lookDuration * 2f)
        {
            timer = 0f;
            lookDuration = Random.Range(1.5f, 3f);
            Debug.Log($"[{monster.name}] 좌우 회전 루프 반복");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: Idle 종료 (회전 초기화)");
    }
}
