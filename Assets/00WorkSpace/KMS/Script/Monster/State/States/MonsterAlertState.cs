using UnityEngine;

public class MonsterAlertState : IMonsterState
{
    private BaseMonster monster;
    private float alertTimer = 0f;
    private float maxAlertDuration = 6f;
    private float returnTriggerRatio = 0.95f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();
        Debug.Log($"[{monster.name}] 상태: Alert 진입");
    }

    public void Execute()
    {
        float distanceFromOrigin = Vector3.Distance(monster.OriginPosition, monster.transform.position);
        //float triggerDistance = monster.ActionRadius * returnTriggerRatio;

        // 95% 경계 도달 시 ReturnWaitState로 전이
        //if (distanceFromOrigin >= triggerDistance)
        //{
        //    Debug.Log($"[{monster.name}] 95% 경계 도달 - 복귀 대기 상태 진입");
        //    monster.StateMachine.ChangeState(new MonsterReturnWaitState());
        //    return;
        //}

        // (이하 기존 Alert 상태 동작)
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(15f);

        //var target = monster.GetTarget();
        //if (target != null)
        //{
        //    Vector3 toTarget = target.position - monster.transform.position;
        //    toTarget.y = 0f;
        //    monster.MoveTo();
        //}
        if (monster.GetTarget() != null)
        {
            monster.MoveTo(monster.GetTarget().position);
        }

        if (monster.IsInAttackRange())
        {
            var attackState = monster.CreateAttackState();
            monster.StateMachine.ChangeState(attackState);
            return;
        }

        alertTimer += Time.deltaTime;
        if (alertTimer >= maxAlertDuration)
        {
            var nextState = monster.StateFactory.GetStateForPerception(monster.GetCurrentPerceptionState());
            monster.StateMachine.ChangeState(nextState);
            Debug.Log($"[{monster.name}] Alert 시간 종료 → 상태 전이: {nextState.GetType().Name}");
        }

        if (monster.AlertLevel < monster.AlertThreshold_Low)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Idle);
            monster.StateMachine.ChangeState(monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Idle));
            Debug.Log($"[{monster.name}] 경계도 하락 → Idle 전이");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Alert 상태 종료");
    }
}