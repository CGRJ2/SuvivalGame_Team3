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
        //Debug.Log($"[{monster.name}] 상태: Alert 진입");
    }

    public void Execute()
    {
        // 공격 범위 체크
        monster.UpdateAttackRange();

        // (이하 기존 Alert 상태 동작)
        if (monster.checkTargetVisible)

            monster.IncreaseAlert(15f);

        // 타겟이 있으면 타겟 위치로 추적이동
        if (monster.GetTarget() != null)
        {
            monster.Agent.SetDestination(monster.GetTarget().position);
        }

        // 공격 범위에 플레이어가 있다면 공격상태로 전환
        if (monster.playerInRange != null)
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
            //Debug.Log($"[{monster.name}] Alert 시간 종료 → 상태 전이: {nextState.GetType().Name}");
        }


        if (monster.AlertLevel < monster.AlertThreshold_Low)
        {
            monster.StateMachine.ChangeState(new MonsterIdleState(monster));
            //Debug.Log($"[{monster.name}] 경계도 하락 → Idle 전이");
        }
    }

    public void Exit()
    {
        //Debug.Log($"[{monster.name}] Alert 상태 종료");
    }
}