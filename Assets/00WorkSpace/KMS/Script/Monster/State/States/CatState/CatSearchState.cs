using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSearchState : IMonsterState
{
    private BaseMonster monster;
    private float searchDuration = 4f;
    private float searchTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        searchTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Search);
        monster.GetComponent<MonsterView>()?.PlayMonsterCautiousWalkAnimation(); // ���� Ž�� ���

        Debug.Log($"[{monster.name}] ����: CatSearch ����");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        searchTimer += Time.deltaTime;

        // �÷��̾ �ٽ� �߰��ϸ� ��赵 ���
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f);
        }

        // ����� ���� �ִ� ���, ������ ����
        var target = monster.GetTarget();
        if (target != null)
        {
            Vector3 toTarget = target.position - monster.transform.position;
            toTarget.y = 0f;
            monster.Move(toTarget.normalized * 0.5f); // �Ϲ� �̵����� ������
        }

        // ���� �ð� �� ���� ���� ��
        if (searchTimer >= searchDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] CatSearch ���� �� {current} ���� ����");
            }
            else
            {
                searchTimer = 0f;
                Debug.Log($"[{monster.name}] CatSearch ���� ����");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ����: CatSearch ����");
    }
}
