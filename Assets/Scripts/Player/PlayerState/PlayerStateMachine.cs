using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerStateMachine
{
    public PlayerState CurState;

    public void Initialize(PlayerState startState)
    {
        CurState = startState;
        CurState?.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == null)
            return;

        if (CurState == newState)
            return;

        CurState.Exit();
        CurState = newState;
        CurState.Enter();
    }

    public void HandleInput() => CurState?.HandleInput();
    public void UpdateLogic() => CurState.UpdateLogic();
    public void FixedUpdateLogic() => CurState.FixedUpdateLogic();
}
public enum PlayerStateType
{
    Idle, Move, Roll, Attack,
    Jump, Fall, AirAttack,
    Hit,
    Dead
}