using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

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

        // ���� ���� üũ
        owner.UpdateAttackRange();

        // Ÿ�� �켱���� Ž�� (�̳� > �÷��̾�)
        OwnerAI.OwnerDetectionTarget targetType = owner.GetClosestTarget(out target);

        if (target == null || targetType == OwnerAI.OwnerDetectionTarget.None)
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= chaseLoseDelay)
            {
                Debug.Log($"[{owner.name}] ���� ���� => Idle ���� ����");
                owner.StateMachine.ChangeState(new OwnerIdleState(owner));
            }
            return;
        }
        else
        {
            lostTimer = 0f;
        }

        Vector3 destination = target.position;
        destination.y = owner.transform.position.y; // y ����

        owner.Agent.speed = owner.OwnerData.MoveSpeed * (targetType == OwnerAI.OwnerDetectionTarget.OwnerBait ? 0.9f : 1.0f);
        owner.Agent.SetDestination(destination);

        if (targetType == OwnerAI.OwnerDetectionTarget.OwnerBait)
        {
            if (Vector3.Distance(owner.transform.position, destination) < 1.2f)
            {
                owner.ApplyPacifyEffect(4f);
                Debug.Log($"[{owner.name}] �̳� ���� => ����ȭ ���� ����");
            }
        }
        else if (targetType == OwnerAI.OwnerDetectionTarget.Player)
        {
            if (owner.playerInRange != null)
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