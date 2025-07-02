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

        monster.SetPerceptionState(MonsterPerceptionState.Search);
        Debug.Log($"[MonsterSearchState] {monster.name} Ž�� ���� ����");
        //monster.StateMachine.SetAnimation("IsSearching", true);
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // �þ� ����
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f); // Ž�� �� �߰� �� ��赵 ����
        }

        // �̵�
        if (monster.GetTarget() != null)
        {
            Vector3 toTarget = monster.GetTarget().position - monster.transform.position;
            toTarget.y = 0f;
            monster.MoveTo(toTarget.normalized);
        }

        // Ÿ�̸� ��� �� ���� ����
        searchTimer += Time.deltaTime;
        if (searchTimer >= searchDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] Ž�� ���� �� {current} ���� ����");
            }
            else
            {
                // ���� �����Ǹ� Ÿ�̸� �ʱ�ȭ (����)
                searchTimer = 0f;
                Debug.Log($"[{monster.name}] Ž�� ���� ����");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[MonsterSearchState] {monster.name} Ž�� ���� ����");
        // monster.StateMachine.SetAnimation("IsSearching", false);
    }
}

