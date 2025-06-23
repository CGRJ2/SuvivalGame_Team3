using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    // 상태를 가지고 있을 것 입니다.
    public Dictionary<T, BaseState> stateDic;
    // 각 상태를 받아서 조건에 따라 상태를 전이시켜 줄것입니다.
    public BaseState CurState;
    public StateMachine()
    {
        stateDic = new Dictionary<T, BaseState>();
    }

    public void ChangeState(BaseState changedState)
    {
        if (CurState == changedState)
            return;

        CurState.Exit(); // 바뀌기 전에 이전 상태에서 탈출.
        CurState = changedState;
        CurState.Enter(); // 바뀐 상태 진입.
    }
    // 각 상태의 Enter, Update, Exit...를 실행시켜줄것입니다.

    public void Update() => CurState.Update();

    public void FixedUpdate() => CurState.FixedUpdate();
}
