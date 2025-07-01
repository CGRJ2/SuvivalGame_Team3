using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSearchState : IMonsterState
{
    private CatAI cat;
    private float searchDuration = 4f;
    private float searchTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        // Ÿ�� ĳ���� Ȯ���ϰ�
        cat = monster as CatAI;
        searchTimer = 0f;

        cat.SetPerceptionState(MonsterPerceptionState.Search);
        cat.GetComponent<MonsterView>()?.PlayMonsterCautiousWalkAnimation();

        Debug.Log($"[{cat.name}] ����: CatSearch ����");
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead) return;

        searchTimer += Time.deltaTime;

        // Ÿ�� ��Ž��
        cat.RefreshBaitList(); // �ʿ信 ���� ���� �ֱ�θ� ȣ��

        Transform target;
        var targetType = cat.GetClosestTarget(out target);

        // ���� ����/����
        if (targetType != CatAI.CatDetectionTarget.None && target != null)
        {
            // ��󺰷� �ൿ �ٸ���
            if (targetType == CatAI.CatDetectionTarget.CatBait)
            {
                // Ĺ���̸� �ٰ����� �Ա�/����ȭ ��
                Vector3 dir = target.position - cat.transform.position;
                dir.y = 0f;
                cat.Move(dir.normalized * 0.5f); // Ž������ õõ��
                // �Դ� �ൿ ���� �߰� ����
            }
            else if (targetType == CatAI.CatDetectionTarget.Player)
            {
                // �÷��̾� �ѱ� or ��赵 ��� ��
                Vector3 dir = target.position - cat.transform.position;
                dir.y = 0f;
                cat.Move(dir.normalized * 0.7f);
                cat.IncreaseAlert(10f);
            }
        }

        // ���� �ð� �� ���� ����
        if (searchTimer >= searchDuration)
        {
            var next = cat.StateFactory.GetStateForPerception(cat.GetCurrentPerceptionState());
            if (next != this)
            {
                cat.StateMachine.ChangeState(next);
                Debug.Log($"[{cat.name}] CatSearch ���� �� ����");
            }
            else
            {
                searchTimer = 0f;
                Debug.Log($"[{cat.name}] CatSearch ���� ����");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] ����: CatSearch ����");
    }
}
