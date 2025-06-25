using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterSuspiciousState : IMonsterState
{
    private BaseMonster monster;
    private float suspicionDuration = 4f;
    private float suspicionTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        suspicionTimer = 0f;

        Debug.Log($"[SuspiciousState] {monster.name} 수상 상태 진입");
        monster.SetPerceptionState(MonsterPerceptionState.Suspicious);
        //monster.StateMachine.SetAnimation("IsSuspicious", true);
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        if (monster.checkTargetVisible) // ? 감지 로직 통일
        {
            monster.IncreaseAlert(5f); // ? 적은 수치 상승
        }

        suspicionTimer += Time.deltaTime;

        if (suspicionTimer >= suspicionDuration)
        {
            MonsterPerceptionState current = monster.GetCurrentPerceptionState(); 

            IMonsterState nextState = monster.StateFactory.GetStateForPerception(current);
            monster.StateMachine.ChangeState(nextState);
        }
    }

    public void Exit()
    {
        Debug.Log($"[SuspiciousState] {monster.name} 수상 상태 종료");
        //monster.StateMachine.SetAnimation("IsSuspicious", false);
    }
}

