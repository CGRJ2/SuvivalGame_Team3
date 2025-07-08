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
        Debug.Log("������� ����");
        monster.view.Animator.SetBool("IsSleep", true);
    }

    public void Execute()
    {
        // ��ħ�̸� �ٽ� �Ϲ� ����
        if (monster is Stalker_Owner stalker)
        {
            if (DailyManager.Instance.currentTimeData.TZ_State.Value == TimeZoneState.Morning)
                monster.StateMachine.ChangeState(stalker.returnToOrigin);
        }
    }

    public void Exit()
    {
        //���� �ִϸ��̼� ����
        monster.view.Animator.SetBool("IsSleep", false);


    }
}
