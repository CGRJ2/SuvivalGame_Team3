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

        monster.view.Animator.SetBool("IsMove", true);

        monster.Agent.SetDestination(stalker.bedTransform.position);
        if (monster.Agent.isOnNavMesh && monster.Agent == true)
            monster.Agent.isStopped = false;
    }

    public void Execute()
    {

        float remainedDistance = (monster.transform.position - monster.Agent.destination).magnitude;
        //Debug.Log($"�����Ÿ� : {remainedDistance}");

        if (remainedDistance <= monster.Agent.stoppingDistance)
            monster.StateMachine.ChangeState(stalker.sleepState);

    }

    public void Exit()
    {
        monster.view.Animator.SetBool("IsMove", false);
    }
}
