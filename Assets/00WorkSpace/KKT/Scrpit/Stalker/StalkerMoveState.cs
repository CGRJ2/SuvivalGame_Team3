using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerMoveState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;
    private Vector3 destination;

    public StalkerMoveState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        // �̵� �ִϸ��̼�
        stalker.anim.SetBool("IsMoving", true);

        // ���� ������ ���� (����: �ݰ� 5 �� ����)
        destination = stalker.transform.position + Random.insideUnitSphere * 5f;
        destination.y = stalker.transform.position.y; // ���� �̵�
    }

    public override void Update()
    {
        // �÷��̾� ������ ��� ����
        stalker.FindTarget();
        if (stalker.target != null)
        {
            controller.ChangeState(StalkerStateType.Chase);
            return;
        }

        // �������� �̵�
        Vector3 dir = (destination - stalker.transform.position).normalized;
        stalker.transform.position += dir * stalker.status.MoveSpeed * Time.deltaTime;

        // ������ ���޽� Idle��
        if (Vector3.Distance(stalker.transform.position, destination) < 0.2f)
        {
            controller.ChangeState(StalkerStateType.Idle);
        }
    }

    public override void Exit()
    {
        stalker.anim.SetBool("IsMoving", false);
    }
}
