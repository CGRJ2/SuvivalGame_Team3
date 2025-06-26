using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IMonsterState
{
    private BaseMonster monster;
    private float alertTimer = 0f;
    private float returnTimer = 0f;
    private bool isWaitingToReturn = false;
    private float returnDelay = 3f;
    private float maxAlertDuration = 6f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        alertTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{monster.name}] 상태: Alert 진입");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead)
        {
            monster.StateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        // 행동 반경 벗어나면 추적 중단 + 대기
        if (monster.IsOutsideActionRadius())
        {
            if (!isWaitingToReturn)
            {
                isWaitingToReturn = true;
                returnTimer = 0f;
                Debug.Log($"[{monster.name}] 반경 이탈 - 복귀 대기 시작");
            }

            returnTimer += Time.deltaTime;

            if (returnTimer >= returnDelay)
            {
                Debug.Log($"[{monster.name}] 복귀 조건 충족 → ReturnState 전환");
                monster.StateMachine.ChangeState(new MonsterReturnState());
                isWaitingToReturn = false;
            }

            monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
            return; // 여기서 경로 종료
        }
        else
        {
            if (isWaitingToReturn)
            {
                Debug.Log($"[{monster.name}] 반경 복귀 → 복귀 대기 취소");
            }

            isWaitingToReturn = false;
            returnTimer = 0f;

            // 이제 여기에서만 추적 이동 수행
            var chasetarget = monster.GetTarget();
            if (chasetarget != null)
            {
                Vector3 toTarget = chasetarget.position - monster.transform.position;
                toTarget.y = 0f;
                monster.Move(toTarget.normalized);
            }
        }

        // 감지 → 경계도 상승
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(15f);
        }

        // 타겟 추적
        var target = monster.GetTarget();
        if (target != null)
        {
            Vector3 toTarget = target.position - monster.transform.position;
            toTarget.y = 0f;
            monster.Move(toTarget.normalized);

            Debug.Log($"[AlertState] 타겟 위치 = {target.position}");
        }
        else
        {
            Debug.LogWarning("[AlertState] 타겟이 없습니다!");
        }

        // 공격 사거리 진입
        if (monster.IsInAttackRange())
        {
            var attackState = monster.CreateAttackState();
            monster.StateMachine.ChangeState(attackState);
            return;
        }

        // 상태 재평가
        alertTimer += Time.deltaTime;
        if (alertTimer >= maxAlertDuration)
        {
            var nextState = monster.StateFactory.GetStateForPerception(monster.GetCurrentPerceptionState());
            if (nextState != this)
            {
                monster.StateMachine.ChangeState(nextState);
                Debug.Log($"[{monster.name}] Alert 시간 종료 → {monster.GetCurrentPerceptionState()} 전이");
            }
            else
            {
                alertTimer = 0f;
                Debug.Log($"[{monster.name}] Alert 상태 유지");
            }
        }

        // 경계도 하락 시 Idle 전이
        if (monster.AlertLevel < monster.AlertThreshold_Low)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Idle);
            var idleState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Idle);
            monster.StateMachine.ChangeState(idleState);
            Debug.Log($"[{monster.name}] 경계도 하락 → Idle 전이");
            return;
        }
    }
    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: Alert 종료");
    }
}
