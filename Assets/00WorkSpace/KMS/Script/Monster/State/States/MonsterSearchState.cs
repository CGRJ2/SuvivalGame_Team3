using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchState : IMonsterState
{
    private BaseMonster monster;
    private float searchDuration = 5f;
    private float searchTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        searchTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Search);
        Debug.Log($"[MonsterSearchState] {monster.name} 탐색 상태 진입");
        //monster.StateMachine.SetAnimation("IsSearching", true);
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // 시야 감지
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f); // 탐색 중 발견 시 경계도 증가
        }

        // 이동
        if (monster.GetTarget() != null)
        {
            Vector3 toTarget = monster.GetTarget().position - monster.transform.position;
            toTarget.y = 0f;
            monster.MoveTo(toTarget.normalized);
        }

        // 타이머 경과 시 상태 재평가
        searchTimer += Time.deltaTime;
        if (searchTimer >= searchDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] 탐색 종료 → {current} 상태 전이");
            }
            else
            {
                // 상태 유지되면 타이머 초기화 (선택)
                searchTimer = 0f;
                Debug.Log($"[{monster.name}] 탐색 상태 유지");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[MonsterSearchState] {monster.name} 탐색 상태 종료");
        // monster.StateMachine.SetAnimation("IsSearching", false);
    }
}

