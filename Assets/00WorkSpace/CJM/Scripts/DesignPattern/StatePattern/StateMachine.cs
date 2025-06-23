using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    // ���¸� ������ ���� �� �Դϴ�.
    public Dictionary<T, BaseState> stateDic;
    // �� ���¸� �޾Ƽ� ���ǿ� ���� ���¸� ���̽��� �ٰ��Դϴ�.
    public BaseState CurState;
    public StateMachine()
    {
        stateDic = new Dictionary<T, BaseState>();
    }

    public void ChangeState(BaseState changedState)
    {
        if (CurState == changedState)
            return;

        CurState.Exit(); // �ٲ�� ���� ���� ���¿��� Ż��.
        CurState = changedState;
        CurState.Enter(); // �ٲ� ���� ����.
    }
    // �� ������ Enter, Update, Exit...�� ��������ٰ��Դϴ�.

    public void Update() => CurState.Update();

    public void FixedUpdate() => CurState.FixedUpdate();
}
