using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IMonsterState
{
    private BaseMonster monster;
    private float alertTimer = 0f;
    private float returnTimer = 0f;
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
            returnTimer += Time.deltaTime;

            if (returnTimer >= returnDelay)
            {
                Debug.Log($"[{monster.name}] 활동 반경 이탈 3초 초과 → Return 상태 전환");
                monster.StateMachine.ChangeState(new MonsterReturnState());
                return;
            }

            monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
            return;
        }
        else
        {
            returnTimer = 0f;
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
