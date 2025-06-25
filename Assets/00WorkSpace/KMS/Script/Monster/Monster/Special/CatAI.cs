public class CatAI : BaseMonster
{
    protected override void Awake()
    {
        {
            stateFactory = new CatMonsterStateFactory(this);
            base.Awake();
        }
    }
    protected override void HandleState()
    {
        if (IsDead)
        {
            if (!(stateMachine.CurrentState is MonsterDeadState))
                stateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        if (CheckTargetVisible())
        {
            var alertState = StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            if (stateMachine.CurrentState != alertState)
                stateMachine.ChangeState(alertState);
        }
        else
        {
            var idleState = StateFactory.GetStateForPerception(MonsterPerceptionState.Idle);
            if (stateMachine.CurrentState != idleState)
                stateMachine.ChangeState(idleState);
        }
    }

    public void ApplyPacifyEffect(float duration)
    {
        // �ܺ� �ڱ�(������ ��)���� ���� ����ȭ ���� ����
        SetPerceptionState(MonsterPerceptionState.Idle); // Ȥ�� Pacified ���� Enum�� ��� ����
        StateMachine.ChangeState(new CatPacifiedState(duration));
    }
}