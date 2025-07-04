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

        Debug.Log($"[{monster.name}] 상태: Chase 진입");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead)
        {
            // Dead 상태 팩토리에서 관리하지 않으면 직접 생성 유지
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
                Debug.Log($"[{monster.name}] 추적 실패 → Idle 전이");
                monster.StateMachine.ChangeState(new MonsterIdleState(monster));
            }
            return;
        }

        lostTimer = 0f;

        // 추적 이동
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

        // 공격 사거리 진입 시 전이
        if (monster.IsInAttackRange())
        {
            var attackState = monster.StateFactory.CreateAttackState();
            monster.StateMachine.ChangeState(attackState);
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] 상태: Chase 종료");
    }
}