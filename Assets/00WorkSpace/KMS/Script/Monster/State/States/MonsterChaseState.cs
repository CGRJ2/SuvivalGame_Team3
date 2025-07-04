using UnityEngine;

public class MonsterChaseState : IMonsterState
{
    private BaseMonster monster;
    private float lostTimer = 0f;
    private const float chaseLoseDelay = 3f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        lostTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{monster.name}] ����: Chase ����");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead)
        {
            // Dead ���� ���丮���� �������� ������ ���� ���� ����
            monster.StateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        //if (monster.IsOutsideActionRadius())
        //{
        //    monster.StateMachine.ChangeState(monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Idle));
        //    return;
        //}

        if (monster.IsOutsideDetectionRadius())
        {
            lostTimer += Time.deltaTime;

            if (lostTimer >= chaseLoseDelay)
            {
                Debug.Log($"[{monster.name}] ���� ���� �� Idle ����");
                monster.StateMachine.ChangeState(new MonsterIdleState(monster));
            }
            return;
        }

        lostTimer = 0f;

        // ���� �̵�
        //if (monster.GetTarget() != null)
        //{
        //    Vector3 toTarget = monster.GetTarget().position - monster.transform.position;
        //    toTarget.y = 0f;
        //    monster.Move(toTarget.normalized);
        //}
        if (monster.GetTarget() != null)
        {
            monster.Agent.SetDestination(monster.GetTarget().position);

        }

        // ���� ��Ÿ� ���� �� ����
        if (monster.IsInAttackRange())
        {
            var attackState = monster.StateFactory.CreateAttackState();
            monster.StateMachine.ChangeState(attackState);
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ����: Chase ����");
    }
}