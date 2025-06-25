public class CatAI : BaseMonster
{
    protected override void HandleState()
    {
        if (IsDead)
        {
            if (!(stateMachine.CurrentState is MonsterDeadState))
                stateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        if (IsInSight())
        {
            SetPerceptionState(MonsterPerceptionState.Alert);
            if (!(stateMachine.CurrentState is MonsterChaseState))
                stateMachine.ChangeState(new MonsterChaseState());
        }
        else
        {
            SetPerceptionState(MonsterPerceptionState.Idle);
            if (!(stateMachine.CurrentState is CatIdleState))
                stateMachine.ChangeState(new CatIdleState());
        }
    }
    
    public void ApplyPacifyEffect(float duration) // ������ ������ ����ȭ �ð��� ���
    {
        SetPerceptionState(MonsterPerceptionState.Idle);
        StateMachine.ChangeState(new CatPacifiedState(duration));
    }
}