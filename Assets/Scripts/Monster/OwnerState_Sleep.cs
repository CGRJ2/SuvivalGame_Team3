using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerState_Sleep :  IMonsterState
{
    protected BaseMonster monster;

    public OwnerState_Sleep(BaseMonster monster)
    {
        this.monster = monster;
    }

    public void Enter(BaseMonster monster)
    {
        Debug.Log("수면상태 진입");
        
        monster.view.Animator.SetBool("IsMove", false);
        monster.Agent.isStopped = true;

        monster.view.Animator.SetTrigger("IsSleep");
    }

    public void Execute()
    {
        // 아침이면 다시 일반 모드로
        if (monster is Stalker_Owner stalker)
        {
            if (DailyManager.Instance.currentTimeData.TZ_State.Value == TimeZoneState.Morning)
                monster.StateMachine.ChangeState(stalker.returnToOrigin);
        }
    }

    public void Exit()
    {

    }
}
