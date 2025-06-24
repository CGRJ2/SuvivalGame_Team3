using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStateMachine
{
    private Dictionary<TutorialStateType, TutorialBaseState> states = new();
    private TutorialBaseState currentState;

    public void AddState(TutorialStateType type, TutorialBaseState state)
    {
        states[type] = state;
    }

    public void ChangeState(TutorialStateType type)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = states[type];
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}

public enum TutorialStateType
{
    Move, Run, Jump, Weapon, Camp, Battle, PostBattle, Attack, Save, Production
}
