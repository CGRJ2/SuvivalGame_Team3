using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBaseState
{
    protected TutorialManager manager;

    public TutorialBaseState(TutorialManager manager)
    {
        this.manager = manager;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
