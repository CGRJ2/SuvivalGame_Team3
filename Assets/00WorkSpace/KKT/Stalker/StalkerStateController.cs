using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum StalkerStateType
{
    Idle,
    Sleep,
    Chase,
    Attack,
}

public class StalkerStateController : MonoBehaviour
{
    public Stalker stalker;
    public StateMachine<StalkerStateType> stateMachine { get; private set; }

    // 각 상태 인스턴스 보관
    public StalkerIdleState idleState { get; private set; }
    public StalkerSleepState sleepState { get; private set; }
    public StalkerChaseState chaseState { get; private set; }
    public StalkerAttackState attackState { get; private set; }

    void Awake()
    {
        if (stalker == null)
            stalker = GetComponent<Stalker>();

        stateMachine = new StateMachine<StalkerStateType>();

        idleState = new StalkerIdleState(stalker, this);
        sleepState = new StalkerSleepState(stalker, this);
        chaseState = new StalkerChaseState(stalker, this);
        attackState = new StalkerAttackState(stalker, this);

        stateMachine.stateDic.Add(StalkerStateType.Idle, idleState);
        stateMachine.stateDic.Add(StalkerStateType.Sleep, attackState);
        stateMachine.stateDic.Add(StalkerStateType.Chase, chaseState);
        stateMachine.stateDic.Add(StalkerStateType.Attack, attackState);

        // 최초 상태 지정
        stateMachine.CurState = new ObservableProperty<BaseState>(idleState);
        idleState.Enter(); // 최초 진입
    }
    void Update()
    {
        stateMachine.Update();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void ChangeState(StalkerStateType newType)
    {
        if (stateMachine.stateDic.TryGetValue(newType, out var nextState))
        {
            stateMachine.ChangeState(nextState);
        }
    }
}
