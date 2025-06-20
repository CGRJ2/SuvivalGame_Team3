using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StalkerState : BaseState
{
    protected Stalker stalker;
    protected StalkerStateController controller;

    public StalkerState(Stalker stalker, StalkerStateController controller)
    {
        this.stalker = stalker;
        this.controller = controller;
    }
}
