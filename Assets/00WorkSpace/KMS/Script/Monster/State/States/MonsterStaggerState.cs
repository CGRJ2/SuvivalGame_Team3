using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStaggerState : IMonsterState
{
    private BaseMonster monster;
    private readonly float duration;
    private float timer;

    public MonsterStaggerState(float duration)
    {
        this.duration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Suspicious); 
        monster.GetComponent<MonsterView>()?.PlayMonsterStaggerAnimation();

        Debug.Log($"[{monster.name}] ����: Stagger ���� ({duration:F1}��)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead)
        {
            monster.StateMachine.ChangeState(new MonsterDeadState()); 
            return;
        }

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            var next = monster.StateFactory.GetStateForPerception(monster.GetCurrentPerceptionState());
            monster.StateMachine.ChangeState(next);

            Debug.Log($"[{monster.name}] Stagger ���� �� {monster.GetCurrentPerceptionState()} ���� ����");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Stagger ���� ����");
    }
}
