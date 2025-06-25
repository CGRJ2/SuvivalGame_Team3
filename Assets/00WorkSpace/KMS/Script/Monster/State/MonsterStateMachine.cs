using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    private IMonsterState currentState;
    public IMonsterState CurrentState => currentState;
    private BaseMonster monster;

    public MonsterStateMachine(BaseMonster owner)
    {
        this.monster = owner;
    }

    public void ChangeState(IMonsterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(monster);
    }

    public void Update()
    {
        currentState?.Execute();
    }
}

