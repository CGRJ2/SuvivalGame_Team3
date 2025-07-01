using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossPhase2AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private BossMonsterDataSO bossData;
    private float attackCooldown;
    private float phase2AttackACooldown;
    private float timer;

    private enum AttackPhase { None, Prelude, Attack, AfterDelay }
    private AttackPhase attackPhase = AttackPhase.None;

    private float timer = 0f;
    private float preludeTime = 1.0f;      // SO���� ����
    private float afterDelay = 0.7f;       // SO���� ����

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        bossData = monster.data as BossMonsterDataSO;

        preludeTime = currentPattern.preludeTime;   // SO���� ���Ϻ� �� �б�
        afterDelay = currentPattern.afterDelay;

        attackPhase = AttackPhase.Prelude;
        timer = 0f;

        monster.GetComponent<MonsterView>()?.PlayMonsterPhase2PreludeAnimation();
        // ���� ����(phase2TryAttack)�� �ݵ�� �ִϸ��̼� �̺�Ʈ���� ȣ��
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        timer += Time.deltaTime;

        if (attackPhase == AttackPhase.Prelude)
        {
            if (timer >= preludeTime)
            {
                attackPhase = AttackPhase.Attack;
                timer = 0f;
                // ���� ���/������ ���� �ִϸ��̼� �̺�Ʈ���� ȣ��
            }
        }
        else if (attackPhase == AttackPhase.Attack)
        {
            // �ִϸ��̼� �̺�Ʈ�� ���� �� attackPhase = AttackPhase.AfterDelay; timer=0;
            // ���÷� �ٷ� �ѱ�ٸ�:
            attackPhase = AttackPhase.AfterDelay;
            timer = 0f;
        }
        else if (attackPhase == AttackPhase.AfterDelay)
        {
            if (timer >= afterDelay)
            {
                // �ĵ� ������ Idle�� ����(Ȥ�� ���� ����)
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase2Attack ����");
    }
}

