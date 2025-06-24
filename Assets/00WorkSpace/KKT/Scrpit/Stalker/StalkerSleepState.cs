using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerSleepState : BaseState
{
    private Stalker stalker;
    private StalkerStateController controller;

    private GameObject player;
    private float runDetectTime = 0f;
    private readonly float wakeUpTime = 3f; // 플레이어가 Run 상태로 3초 이상 있으면 깨어남

    public StalkerSleepState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }

    public override void Enter()
    {
        stalker.anim.SetBool("IsMoving", false); // Idle 애니메이션
        player = null;
        runDetectTime = 0f;
    }

    public override void Update()
    {
        // 플레이어가 범위 내에 있는지 감지
        Collider[] cols = Physics.OverlapSphere(stalker.transform.position, stalker.status.ColliderRange, LayerMask.GetMask("Player"));

        if (cols.Length > 0)
        {
            player = cols[0].gameObject;
            // 플레이어가 Run 상태인지 체크 (예: PlayerController에 isRunning 변수)
            var playerController = player.GetComponent<PlayerController>();
            if (true/*playerController != null && playerController.IsRunning*/)
            {
                runDetectTime += Time.deltaTime;

                if (runDetectTime >= wakeUpTime)
                {
                    // 플레이어를 타겟으로 설정
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