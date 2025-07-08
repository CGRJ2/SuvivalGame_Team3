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
        //monster.GetComponent<MonsterView>()?.PlayMonsterSuspiciousAnimation(); 수면 애니메이션 실행
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        // 주인 전용
        if (monster is Stalker_Owner stalker)
        {
            monster.StateMachine.ChangeState(stalker.returnToBed);
        }
    }
}
