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
        attackCooldown = monster.data.AttackCooldown; // SO ��� �޾ƿ���
        monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();
        timer = 0f;
    }

    public void Execute()     // ��Ÿ�� ����, ���� ���� ���� üũ
    {
        if (monster == null || monster.IsDead) return;

        // ���� ��Ÿ� üũ
        if (!monster.IsInAttackRange())
        {
            // ���� ����(�Ǵ� Alert ����)�� ��ȯ
            var chaseState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            monster.StateMachine.ChangeState(chaseState);
            return;
        }
        // �������� ������ ������ ȸ�� + Idle����
        if (monster.IsOutsideActionRadius())
        {
            bossMonster.ResetBoss();
            monster.StateMachine.ChangeState(new MonsterIdleState());
            return;
        }
        // ���� ��Ÿ��
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;
            monster.TryAttack();
            monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();
        }
    }


    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack ����");
    }
}
