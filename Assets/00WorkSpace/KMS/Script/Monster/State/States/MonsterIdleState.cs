using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterIdleState : IMonsterState
{
    private BaseMonster monster;
    private float idleTime;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        idleTime = Random.Range(1f, 3f); // 랜덤 대기

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{monster.name}] 상태: Idle 진입 (대기 {idleTime:F1}s)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // 플레이어가 보이면 경계도 상승
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f);
        }

        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            var currentState = monster.GetCurrentPerceptionState();
            var nextState = monster.StateFactory.GetStateForPerception(currentState);

            // 현재 Idle 상태인데 상태 변화가 감지되었을 경우 전이
            if (nextState != this)
            {
                monster.StateMachine.ChangeState(nextState);
                Debug.Log($"[{monster.name}] Idle 시간 경과 → {currentState} 상태 전이");
            }
            else
            {
                // 재랜덤 타이머 갱신
                timer = 0f;
                idleTime = Random.Range(1f, 3f);
                Debug.Log($"[{monster.name}] Idle 유지 (새 idleTime: {idleTime:F1}s)");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: Idle 종료");
    }
}


