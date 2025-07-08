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
        if (monster.Agent.isOnNavMesh)
            monster.Agent.isStopped = false;


        monster.Agent.SetDestination(stalker.bedTransform.position);

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
       
    }
}
