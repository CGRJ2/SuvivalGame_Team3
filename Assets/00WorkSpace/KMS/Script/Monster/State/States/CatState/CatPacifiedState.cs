using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPacifiedState : IMonsterState
{
    private CatAI cat;
    private float pacifyDuration;
    private float timer;

    public CatPacifiedState(float duration)
    {
        pacifyDuration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        timer = 0f;

        // 무력화 상태에서는 경계도/퍼셉션 모두 Idle로 유지
        cat.SetPerceptionState(MonsterPerceptionState.Idle);
        cat.GetComponent<MonsterView>()?.PlayMonsterPacifyAnimation();

        Debug.Log($"[{cat.name}] 상태: CatPacified 진입 ({pacifyDuration:F1}초)");
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead) return;

        timer += Time.deltaTime;

        if (timer >= pacifyDuration)
        {
            // 무력화 상태 종료 → 현 퍼셉션 상태 기반으로 상태전이
            var next = cat.StateFactory.GetStateForPerception(cat.GetCurrentPerceptionState());

            cat.StateMachine.ChangeState(next);
            Debug.Log($"[{cat.name}] 무력화 종료 → {next.GetType().Name} 상태 전이");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] 상태: CatPacified 종료");
    }
}