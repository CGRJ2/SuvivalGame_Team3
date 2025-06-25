public class CatAI : BaseMonster
{
    protected override void Awake()
    {
        {
            base.Awake();
            stateFactory = new CatMonsterStateFactory(this);
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

        // ����̴� passively ���� => �þ� ��� ����
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