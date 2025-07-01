using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSearchState : IMonsterState
{
    private CatAI cat;
    private float searchDuration = 4f;
    private float searchTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        // 타입 캐스팅 확실하게
        cat = monster as CatAI;
        searchTimer = 0f;

        cat.SetPerceptionState(MonsterPerceptionState.Search);
        cat.GetComponent<MonsterView>()?.PlayMonsterCautiousWalkAnimation();

        Debug.Log($"[{cat.name}] 상태: CatSearch 진입");
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead) return;

        searchTimer += Time.deltaTime;

        // 타겟 재탐색
        cat.RefreshBaitList(); // 필요에 따라 일정 주기로만 호출

        Transform target;
        var targetType = cat.GetClosestTarget(out target);

        // 실제 감지/추적
        if (targetType != CatAI.CatDetectionTarget.None && target != null)
        {
            // 대상별로 행동 다르게
            if (targetType == CatAI.CatDetectionTarget.CatBait)
            {
                // 캣잎이면 다가가서 먹기/무력화 등
                Vector3 dir = target.position - cat.transform.position;
                dir.y = 0f;
                cat.Move(dir.normalized * 0.5f); // 탐색중은 천천히
                // 먹는 행동 조건 추가 가능
            }
            else if (targetType == CatAI.CatDetectionTarget.Player)
            {
                // 플레이어 쫓기 or 경계도 상승 등
                Vector3 dir = target.position - cat.transform.position;
                dir.y = 0f;
                cat.Move(dir.normalized * 0.7f);
                cat.IncreaseAlert(10f);
            }
        }

        // 일정 시간 후 상태 전이
        if (searchTimer >= searchDuration)
        {
            var next = cat.StateFactory.GetStateForPerception(cat.GetCurrentPerceptionState());
            if (next != this)
            {
                cat.StateMachine.ChangeState(next);
                Debug.Log($"[{cat.name}] CatSearch 종료 → 전이");
            }
            else
            {
                searchTimer = 0f;
                Debug.Log($"[{cat.name}] CatSearch 상태 유지");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] 상태: CatSearch 종료");
    }
}
