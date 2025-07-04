using UnityEngine;

public class BossPhase1AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private float attackCooldown;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        attackCooldown = monster.data.AttackCooldown; // SO 등에서 받아오기
        timer = 0f;
    }

    public void Execute()     // 쿨타임 감소, 공격 가능 여부 체크
    {
        if (monster == null || monster.IsDead) return;

        // 공격 사거리 체크
        if (!monster.IsInAttackRange())
        {
            // 추적 상태(또는 Alert 상태)로 전환
            var chaseState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(chaseState);
            return;
        }
        // 활동범위 밖으로 나가면 회복 + Idle상태
        if (monster.IsOutsideDetectionRadius())
        {
            monster.StateMachine.ChangeState(new MonsterIdleState(bossMonster));
            return;
        }
        // 공격 쿨타임
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;
            monster.TryAttack();
        }
    }


    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack 종료");
    }
}
