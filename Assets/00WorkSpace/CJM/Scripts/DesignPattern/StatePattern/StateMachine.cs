using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    // ���¸� ������ ���� �� �Դϴ�.
    public Dictionary<T, BaseState> stateDic;
    // �� ���¸� �޾Ƽ� ���ǿ� ���� ���¸� ���̽��� �ٰ��Դϴ�.
    public ObservableProperty<BaseState> CurState;
    public StateMachine()
    {
        stateDic = new Dictionary<T, BaseState>();
    }

    public void ChangeState(BaseState changedState)
    {
        if (CurState.Value == changedState)
            return;

        CurState.Value.Exit(); // �ٲ�� ���� ���� ���¿��� Ż��.
        CurState.Value = changedState;
        CurState.Value.Enter(); // �ٲ� ���� ����.
    }
    // �� ������ Enter, Update, Exit...�� ��������ٰ��Դϴ�.

    public void Update() => CurState.Value.Update();

    public void FixedUpdate() => CurState.Value.FixedUpdate();
}
