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

        Debug.Log($"[MonsterSuspiciousState] {monster.name} ���� ���� ����");
        monster.SetPerceptionState(MonsterPerceptionState.Suspicious);
        //monster.StateMachine.SetAnimation("IsSuspicious", true);
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // �÷��̾ ���̸� ��赵 ���
        if (monster.IsInSight())
        {
            monster.IncreaseAlert(5f); // ���� ����
        }

        suspicionTimer += Time.deltaTime;

        if (suspicionTimer >= suspicionDuration)
        {
            MonsterPerceptionState current = monster.EvaluateCurrentAlertState();

            if (current == MonsterPerceptionState.Idle)
            {
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }
            else if (current == MonsterPerceptionState.Search)
            {
                monster.StateMachine.ChangeState(monster.GetSearchState());
            }
            else if (current == MonsterPerceptionState.Alert)
            {
                monster.StateMachine.ChangeState(monster.GetAlertState());
            }
        }
    }

    public void Exit()
    {
        //monster.StateMachine.SetAnimation("IsSuspicious", false);
        Debug.Log($"[MonsterSuspiciousState] {monster.name} ���� ���� ����");
    }
}

