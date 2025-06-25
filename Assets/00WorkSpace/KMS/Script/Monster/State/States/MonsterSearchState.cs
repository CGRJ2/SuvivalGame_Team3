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

        Debug.Log($"[MonsterSearchState] {monster.name} 탐색 상태 진입");
        monster.SetPerceptionState(MonsterPerceptionState.Search);
        //monster.StateMachine.SetAnimation("IsSearching", true); 
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // 플레이어를 시야에 포착했는지 검사
        if (monster.IsInSight())
        {
            monster.IncreaseAlert(10f); // 경계도 추가 상승
        }

        Vector3 toPlayer = monster.GetTarget().position - monster.transform.position;
        toPlayer.y = 0;
        monster.Move(toPlayer.normalized);

        // 일정 시간 후 경계도 수준에 따라 상태 전이
        searchTimer += Time.deltaTime;
        if (searchTimer >= searchDuration)
        {
            MonsterPerceptionState current = monster.EvaluateCurrentAlertState();
            if (current == MonsterPerceptionState.Idle)
            {
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }
            else if (current == MonsterPerceptionState.Alert)
            {
                monster.StateMachine.ChangeState(monster.GetAlertState());
            }

            // Suspicious는 유지 or 처리 방식에 따라 여기서 확장 가능
        }
    }

    public void Exit()
    {
        //monster.StateMachine.SetAnimation("IsSearching", false);
        Debug.Log($"[MonsterSearchState] {monster.name} 탐색 상태 종료");
    }
}
