using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : IMonsterState
{
    private BaseMonster monster;
    private bool animationPlayed = false;

    public void Enter(BaseMonster monster)
    {
        Debug.Log($"[{monster.name}] 상태: Dead 진입");
        this.monster = monster;
        animationPlayed = false;
        monster.Agent.enabled = false;
        if (monster.IsDead)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterDeathAnimation();
            animationPlayed = true;
            foreach (var rigid in monster.GetComponentsInChildren<Rigidbody>())
            {
                rigid.isKinematic = true;
                rigid.detectCollisions = false;
            }
        }
        else
        {
            Debug.LogWarning($"[{monster.name}] Dead 상태 진입 요청 → 그러나 isDead == false");
        }
        PlayerManager.Instance.instancePlayer.Status.ChargeBattery(SuvivalSystemManager.Instance.batterySystem.RecoverAmount_MonsterSlay);


        monster.view.Animator.SetBool("IsMove", false);

        monster.Agent.isStopped = true;
    }

    public void Execute()
    {
        // 사망 상태에서는 아무것도 하지 않음
        // 죽음 애니메이션 완료 후 파괴 등 타이밍 기반 처리 필요 시 여기에 작성
    }

    public void Exit()
    {
        // 죽음 상태는 종료되지 않으므로 특별한 처리 없음
        Debug.Log($"[{monster.name}] Dead 상태에서 Exit 호출 (예외적 상황일 수 있음)");
        monster.view.Animator.SetBool("IsMove", true);
        if (monster.Agent.isOnNavMesh)
            monster.Agent.isStopped = false;
    }
}