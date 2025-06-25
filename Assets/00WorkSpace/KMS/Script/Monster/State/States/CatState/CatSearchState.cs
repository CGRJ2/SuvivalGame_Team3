using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSearchState : IMonsterState
{
    private BaseMonster monster;
    private float searchDuration = 4f;
    private float searchTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        searchTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Search);
        monster.GetComponent<MonsterView>()?.PlayMonsterCautiousWalkAnimation(); // 느린 탐색 모션

        Debug.Log($"[{monster.name}] 상태: CatSearch 진입");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        searchTimer += Time.deltaTime;

        // 플레이어를 다시 발견하면 경계도 상승
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f);
        }

        // 대상이 아직 있는 경우, 서서히 접근
        var target = monster.GetTarget();
        if (target != null)
        {
            Vector3 toTarget = target.position - monster.transform.position;
            toTarget.y = 0f;
            monster.Move(toTarget.normalized * 0.5f); // 일반 이동보다 느리게
        }

        // 일정 시간 후 상태 전이 평가
        if (searchTimer >= searchDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] CatSearch 종료 → {current} 상태 전이");
            }
            else
            {
                searchTimer = 0f;
                Debug.Log($"[{monster.name}] CatSearch 상태 유지");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: CatSearch 종료");
    }
}
