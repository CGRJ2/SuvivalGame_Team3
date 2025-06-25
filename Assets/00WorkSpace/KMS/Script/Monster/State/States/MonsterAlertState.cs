using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IMonsterState
{
    private BaseMonster monster;
    private float alertDuration = 8f;
    private float alertTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        alertTimer = 0f;

        Debug.Log($"[MonsterAlertState] {monster.name} 경계 상태 진입");
        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        //monster.StateMachine.SetAnimation("IsAlert", true); 
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // 시야 내에 플레이어가 있다면 경계도 유지
        if (monster.IsInSight())
        {
            monster.IncreaseAlert(10f);
        }

        alertTimer += Time.deltaTime;

        // 일정 시간 후 경계 상태 유지 여부 판단
        if (alertTimer >= alertDuration)
        {
            MonsterPerceptionState current = monster.EvaluateCurrentAlertState();

            if (current == MonsterPerceptionState.Search)
            {
                monster.StateMachine.ChangeState(monster.GetSearchState());
            }
            else if (current == MonsterPerceptionState.Idle)
            {
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }

            // Suspicious 상태가 존재한다면 여기에 포함 가능
        }
    }

    public void Exit()
    {
        //monster.StateMachine.SetAnimation("IsAlert", false);
        Debug.Log($"[MonsterAlertState] {monster.name} 경계 상태 종료");
    }
}
