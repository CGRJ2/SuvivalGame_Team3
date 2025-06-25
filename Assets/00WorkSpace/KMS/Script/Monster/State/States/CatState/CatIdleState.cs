using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdleState : IMonsterState
{
    private BaseMonster monster;
    private float idleDuration;
    private float idleTimer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        idleTimer = 0f;
        idleDuration = Random.Range(2f, 5f); // 고양이는 좀 오래 앉아 있음

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{monster.name}] 상태: CatIdle 진입 (대기 {idleDuration:F1}s)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        idleTimer += Time.deltaTime;

        // 플레이어를 감지하면 경계도 상승
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(8f); // 일반 몬스터보다 낮은 수치 상승
        }

        // 시간이 지나면 상태 전이 판단
        if (idleTimer >= idleDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] CatIdle → {current} 상태 전이");
            }
            else
            {
                // 여전히 Idle 상태라면 랜덤 타이머 재설정
                idleTimer = 0f;
                idleDuration = Random.Range(2f, 5f);
                Debug.Log($"[{monster.name}] CatIdle 상태 유지 (새 타이머: {idleDuration:F1}s)");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: CatIdle 종료");
    }
}
