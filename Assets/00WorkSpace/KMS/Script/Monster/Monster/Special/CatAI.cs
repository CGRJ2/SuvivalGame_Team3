public class CatAI : BaseMonster
{
    protected override void HandleState()
    {
        if (IsDead)
        {
            if (!(stateMachine.CurrentState is DeadState))
                stateMachine.ChangeState(new DeadState());
            return;
        }

        if (IsInSight())
        {
            SetPerceptionState(MonsterPerceptionState.Alert);
            if (!(stateMachine.CurrentState is ChaseState))
                stateMachine.ChangeState(new ChaseState());
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