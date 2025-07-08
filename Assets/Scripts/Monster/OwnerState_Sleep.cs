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
        //monster.GetComponent<MonsterView>()?.PlayMonsterSuspiciousAnimation(); ���� �ִϸ��̼� ����
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        // ���� ����
        if (monster is Stalker_Owner stalker)
        {
            monster.StateMachine.ChangeState(stalker.returnToBed);
        }
    }
}
