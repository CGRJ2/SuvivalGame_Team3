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

        // 고양이는 passively 관찰 => 시야 기반 감지
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
        // 외부 자극(아이템 등)으로 인해 무력화 상태 진입
        SetPerceptionState(MonsterPerceptionState.Idle); // 혹은 Pacified 전용 Enum도 고려 가능
        StateMachine.ChangeState(new CatPacifiedState(duration));
    }
}