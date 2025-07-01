using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdleState : IMonsterState
{
    private CatAI cat;
    private float idleDuration;
    private float idleTimer;

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        idleTimer = 0f;
        idleDuration = Random.Range(2f, 5f); // 고양이는 좀 오래 앉아 있음

        cat.SetPerceptionState(MonsterPerceptionState.Idle);
        cat.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{cat.name}] 상태: CatIdle 진입 (대기 {idleDuration:F1}s)");
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead) return;

        idleTimer += Time.deltaTime;

        // CatAI의 감지 로직 활용
        Transform target;
        var targetType = cat.GetClosestTarget(out target);

        // 플레이어나 캣잎 등 감지 시 경계도 소폭 상승
        if (targetType != CatAI.CatDetectionTarget.None)
        {
            cat.IncreaseAlert(8f); // 낮은 수치
        }

        // 시간이 지나면 상태 전이 판단
        if (idleTimer >= idleDuration)
        {
            var next = cat.StateFactory.GetStateForPerception(cat.GetCurrentPerceptionState());

            if (next != this)
            {
                cat.StateMachine.ChangeState(next);
                Debug.Log($"[{cat.name}] CatIdle → {next.GetType().Name} 상태 전이");
            }
            else
            {
                // 여전히 Idle 상태라면 랜덤 타이머 재설정
                idleTimer = 0f;
                idleDuration = Random.Range(2f, 5f);
                Debug.Log($"[{cat.name}] CatIdle 상태 유지 (새 타이머: {idleDuration:F1}s)");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] 상태: CatIdle 종료");
    }
}