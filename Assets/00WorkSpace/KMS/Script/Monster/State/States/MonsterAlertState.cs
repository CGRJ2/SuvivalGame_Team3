using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IMonsterState
{
    private BaseMonster monster;
    private float alertDuration = 8f;
    private float alertTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        alertTimer = 0f;

        Debug.Log($"[MonsterAlertState] {monster.name} ��� ���� ����");
        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        //monster.StateMachine.SetAnimation("IsAlert", true); 
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // �þ� ���� �÷��̾ �ִٸ� ��赵 ����
        if (monster.IsInSight())
        {
            monster.IncreaseAlert(10f);
        }

        alertTimer += Time.deltaTime;

        // ���� �ð� �� ��� ���� ���� ���� �Ǵ�
        if (alertTimer >= alertDuration)
        {
            MonsterPerceptionState current = monster.EvaluateCurrentAlertState();

            if (current == MonsterPerceptionState.Search)
            {
                monster.StateMachine.ChangeState(monster.GetSearchState());
            }
            else if (current == MonsterPerceptionState.Idle)
            {
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }

            // Suspicious ���°� �����Ѵٸ� ���⿡ ���� ����
        }
    }

    public void Exit()
    {
        //monster.StateMachine.SetAnimation("IsAlert", false);
        Debug.Log($"[MonsterAlertState] {monster.name} ��� ���� ����");
    }
}
