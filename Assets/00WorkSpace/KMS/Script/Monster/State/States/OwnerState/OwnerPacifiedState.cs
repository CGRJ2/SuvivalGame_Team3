using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerPacifiedState : IMonsterState
{
    private BaseMonster monster;
    private float duration;
    private float timer;

    public OwnerPacifiedState(float duration)
    {
        this.duration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        monster.GetComponent<MonsterView>()?.PlayMonsterPacifyAnimation(); // 애니메이터 트리거
        Debug.Log($"[{monster.name}] OwnerStun 진입 (6초)");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            // 복귀 조건 판단
            var next = monster.GetCurrentPerceptionState() == MonsterPerceptionState.Alert
                ? monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert)
                : monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Idle);
            monster.StateMachine.ChangeState(next);
            Debug.Log($"[{monster.name}] 무력화 종료 → 상태 복귀");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] OwnerStun 종료");
    }
}
