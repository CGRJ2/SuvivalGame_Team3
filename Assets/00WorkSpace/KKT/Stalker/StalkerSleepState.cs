using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerSleepState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;

    private GameObject player;
    private float runDetectTime = 0f;
    private readonly float wakeUpTime = 3f; // �÷��̾ Run ���·� 3�� �̻� ������ ���

    public StalkerSleepState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        stalker.anim.SetBool("IsMoving", false); // Idle �ִϸ��̼�
        player = null;
        runDetectTime = 0f;
    }

    public override void Update()
    {
        // �÷��̾ ���� ���� �ִ��� ����
        Collider[] cols = Physics.OverlapSphere(stalker.transform.position, stalker.status.ColliderRange, LayerMask.GetMask("Player"));

        if (cols.Length > 0)
        {
            player = cols[0].gameObject;
            // �÷��̾ Run �������� üũ (��: PlayerController�� isRunning ����)
            var playerController = player.GetComponent<PlayerController>();
            if (true/*playerController != null && playerController.IsRunning*/)
            {
                runDetectTime += Time.deltaTime;

                if (runDetectTime >= wakeUpTime)
                {
                    // �÷��̾ Ÿ������ ����
                    stalker.target = player;
                    controller.ChangeState(StalkerStateType.Chase);
                }
            }
            else
            {
                runDetectTime = 0f;
            }
        }
        else
        {
            player = null;
            runDetectTime = 0f;
        }
    }

    public override void Exit()
    {
        runDetectTime = 0f;
        player = null;
    }
}