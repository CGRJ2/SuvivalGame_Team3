using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerAttackState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;
    private bool isAttacking = false;

    public StalkerAttackState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        isAttacking = false;
        stalker.rigid.velocity = Vector3.zero; // ������ ����
        stalker.anim.SetBool("IsMoving", false);
        stalker.Attack(); // �ִϸ��̼� Ʈ����, �������� �ִϸ��̼� �̺�Ʈ����
        isAttacking = true;
    }

    public override void Update()
    {
        if (stalker.target == null)
        {
            controller.ChangeState(StalkerStateType.Idle);
            return;
        }

        float distance = Vector3.Distance(stalker.transform.position, stalker.target.transform.position);

        // ��Ÿ� ������ ����� ���� ���·�
        if (distance > stalker.status.AttackRange)
        {
            controller.ChangeState(StalkerStateType.Chase);
            return;
        }

        // ���� ��Ÿ��/�ִϸ��̼� ���� � ���� ����� ���� �߰� ����
        // ����: ���� ��Ÿ�� �� �����, �Ǵ� �ִϸ��̼� �̺�Ʈ���� ChangeState ȣ��
    }

    public override void Exit() { }
}
