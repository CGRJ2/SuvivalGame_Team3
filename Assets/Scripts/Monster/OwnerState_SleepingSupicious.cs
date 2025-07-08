using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerState_SleepingSupicious : IMonsterState
{
    private BaseMonster monster;

    // 필드에서 조정할 수 있게 수정 필요
    private float stateDuration = 3f;
    private float stateTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        stateTimer = 0f;

        //Debug.Log($"[Sleeping SuspiciousState] {monster.name} 수면 중 수상 상태 진입");
        monster.SetPerceptionState(MonsterPerceptionState.Suspicious);
        //monster.GetComponent<MonsterView>()?.PlayMonsterSuspiciousAnimation();
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        stateTimer += Time.deltaTime;

        // Suspicious 상태에서는 경계도만 점진적으로 상승
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(Time.deltaTime * 15f);

        // Suspicious 최소 유지 시간 경과 후에만 상태 평가
        if (stateTimer >= stateDuration)
        {
            // Perception 평가(경계도+조건 등)
            MonsterPerceptionState nextPerception = monster.GetCurrentPerceptionState();

            if (nextPerception == MonsterPerceptionState.Alert)
            {
                monster.StateMachine.ChangeState(monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert));
                Debug.Log($"[{monster.name}] Sleeping Suspicious → Alert 상태 전이");
            }
            else
            {
                stateTimer = 0;
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[SuspiciousState] {monster.name} 수상 상태 종료");
    }
}
