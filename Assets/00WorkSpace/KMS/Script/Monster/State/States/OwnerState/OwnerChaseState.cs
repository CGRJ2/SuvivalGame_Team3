using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerChaseState : IMonsterState
{
    private OwnerAI owner;
    private Transform target;
    private float lostTimer = 0f;
    private const float chaseLoseDelay = 2.5f;

    public void Enter(BaseMonster monster)
    {
        owner = monster as OwnerAI;
        target = null;
        lostTimer = 0f;
        owner.RefreshBaitList();
        owner.SetPerceptionState(MonsterPerceptionState.Alert);
        owner.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{owner.name}] ����: OwnerChase ����");
    }

    public void Execute()
    {
        if (owner == null || owner.IsDead) return;

        // Ÿ�� �켱���� Ž�� (�̳� > �÷��̾�)
        OwnerAI.OwnerDetectionTarget targetType = owner.GetClosestTarget(out target);

        if (target == null || targetType == OwnerAI.OwnerDetectionTarget.None)
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= chaseLoseDelay)
            {
                Debug.Log($"[{owner.name}] ���� ���� => Idle ���� ����");
                owner.StateMachine.ChangeState(new OwnerIdleState());
            }
            return;
        }
        else
        {
            lostTimer = 0f;
        }

        // �̳��� �÷��̾� �б�
        Vector3 toTarget = target.position - owner.transform.position;
        toTarget.y = 0;

        if (targetType == OwnerAI.OwnerDetectionTarget.OwnerBait)
        {
            owner.Move(toTarget.normalized * 1f); // �̳� �ӵ����� ����
            if (toTarget.magnitude < 1.2f)
            {
                owner.ApplyPacifyEffect(6f);  
                Debug.Log($"[{owner.name}] �̳� ���� => ����ȭ ���� ����");
            }
        }
        else if (targetType == OwnerAI.OwnerDetectionTarget.Player)
        {
            owner.Move(toTarget.normalized * 1.0f);
            // ���� ��Ÿ� ���� �� ��� ��
            if (owner.IsInAttackRange())
            {
                Debug.Log($"[{owner.name}] �÷��̾� ���� => ��� ���� ���� ����");
                owner.StateMachine.ChangeState(owner.GetAttackState());
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{owner.name}] OwnerChase ����");
    }
}