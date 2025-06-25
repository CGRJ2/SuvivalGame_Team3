using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPacifiedState : IMonsterState
{
    private BaseMonster monster;
    private float pacifyDuration;
    private float timer;

    public CatPacifiedState(float duration)
    {
        pacifyDuration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Idle); // 무력화 중에는 경계도 초기화 or 유지
        monster.GetComponent<MonsterView>()?.PlayMonsterPacifyAnimation();

        Debug.Log($"[{monster.name}] 상태: CatPacified 진입 ({pacifyDuration:F1}초)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        timer += Time.deltaTime;

        if (timer >= pacifyDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            monster.StateMachine.ChangeState(next);
            Debug.Log($"[{monster.name}] 무력화 종료 → {current} 상태 전이");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: CatPacified 종료");
    }
}

