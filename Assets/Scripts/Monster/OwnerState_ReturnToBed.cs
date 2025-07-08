using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerState_ReturnToBed : IMonsterState
{
    protected BaseMonster monster;
    private Stalker_Owner stalker;

    public OwnerState_ReturnToBed(BaseMonster monster)
    {
        this.monster = monster;
        stalker = monster as Stalker_Owner;
    }

    public void Enter(BaseMonster monster)
    {
        Debug.Log("���ڷ� ���ư��� ��");

        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        monster.Agent.SetDestination(stalker.bedTransform.position);
    }

    public void Execute()
    {
        float remainedDistance = (monster.transform.position - monster.Agent.destination).magnitude;

        if (remainedDistance < 0.5f)
            monster.StateMachine.ChangeState(stalker.sleepState);

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
