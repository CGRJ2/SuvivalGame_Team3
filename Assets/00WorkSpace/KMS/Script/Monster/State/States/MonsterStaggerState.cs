using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStaggerState : IMonsterState
{
    private BaseMonster monster;
    private float duration;
    private float timer;

    public MonsterStaggerState(float duration)
    {
        this.duration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;

        monster.GetComponent<MonsterView>()?.PlayMonsterStaggerAnimation();
        Debug.Log($"[{monster.name}] ����: Stagger ���� ({duration}��)");
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (monster.IsDead)
        {
            monster.StateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        if (timer >= duration)
        {
            monster.StateMachine.ChangeState(new MonsterIdleState()); // �Ǵ� ���� ���� ����
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Stagger ���� ����");
    }
}
