using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchState : IMonsterState
{
    private BaseMonster monster;
    private float searchDuration = 5f;
    private float searchTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        searchTimer = 0f;

        Debug.Log($"[MonsterSearchState] {monster.name} Ž�� ���� ����");
        monster.SetPerceptionState(MonsterPerceptionState.Search);
        //monster.StateMachine.SetAnimation("IsSearching", true); 
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // �÷��̾ �þ߿� �����ߴ��� �˻�
        if (monster.IsInSight())
        {
            monster.IncreaseAlert(10f); // ��赵 �߰� ���
        }

        Vector3 toPlayer = monster.GetTarget().position - monster.transform.position;
        toPlayer.y = 0;
        monster.Move(toPlayer.normalized);

        // ���� �ð� �� ��赵 ���ؿ� ���� ���� ����
        searchTimer += Time.deltaTime;
        if (searchTimer >= searchDuration)
        {
            MonsterPerceptionState current = monster.EvaluateCurrentAlertState();
            if (current == MonsterPerceptionState.Idle)
            {
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }
            else if (current == MonsterPerceptionState.Alert)
            {
                monster.StateMachine.ChangeState(monster.GetAlertState());
            }

            // Suspicious�� ���� or ó�� ��Ŀ� ���� ���⼭ Ȯ�� ����
        }
    }

    public void Exit()
    {
        //monster.StateMachine.SetAnimation("IsSearching", false);
        Debug.Log($"[MonsterSearchState] {monster.name} Ž�� ���� ����");
    }
}
