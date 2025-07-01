using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdleState : IMonsterState
{
    private CatAI cat;
    private float idleDuration;
    private float idleTimer;

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        idleTimer = 0f;
        idleDuration = Random.Range(2f, 5f); // ����̴� �� ���� �ɾ� ����

        cat.SetPerceptionState(MonsterPerceptionState.Idle);
        cat.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{cat.name}] ����: CatIdle ���� (��� {idleDuration:F1}s)");
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead) return;

        idleTimer += Time.deltaTime;

        // CatAI�� ���� ���� Ȱ��
        Transform target;
        var targetType = cat.GetClosestTarget(out target);

        // �÷��̾ Ĺ�� �� ���� �� ��赵 ���� ���
        if (targetType != CatAI.CatDetectionTarget.None)
        {
            cat.IncreaseAlert(8f); // ���� ��ġ
        }

        // �ð��� ������ ���� ���� �Ǵ�
        if (idleTimer >= idleDuration)
        {
            var next = cat.StateFactory.GetStateForPerception(cat.GetCurrentPerceptionState());

            if (next != this)
            {
                cat.StateMachine.ChangeState(next);
                Debug.Log($"[{cat.name}] CatIdle �� {next.GetType().Name} ���� ����");
            }
            else
            {
                // ������ Idle ���¶�� ���� Ÿ�̸� �缳��
                idleTimer = 0f;
                idleDuration = Random.Range(2f, 5f);
                Debug.Log($"[{cat.name}] CatIdle ���� ���� (�� Ÿ�̸�: {idleDuration:F1}s)");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] ����: CatIdle ����");
    }
}