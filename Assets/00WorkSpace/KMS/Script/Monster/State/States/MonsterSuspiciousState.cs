using UnityEngine;

public class MonsterSuspiciousState : IMonsterState
{
    private BaseMonster monster;
    private float stateDuration = 3f;
    private float stateTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        stateTimer = 0f;

        //Debug.Log($"[SuspiciousState] {monster.name} 수상 상태 진입");
        
        monster.Agent.isStopped = true;

        monster.SetPerceptionState(MonsterPerceptionState.Suspicious);
        monster.view.Animator.SetTrigger("Idle");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        stateTimer += Time.deltaTime;

        // Suspicious 상태에서는 경계도만 점진적으로 상승
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(Time.deltaTime * 15f);

        // Suspicious 최소 유지 시간 경과 후에만 상태 평가
        if (stateTimer >= stateDuration)
        {
            // Perception 평가(경계도+조건 등)
            MonsterPerceptionState nextPerception = monster.GetCurrentPerceptionState();

            if (nextPerception == MonsterPerceptionState.Alert)
            {
                monster.StateMachine.ChangeState(monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert));
                //Debug.Log($"[{monster.name}] Suspicious → Alert 상태 전이");
            }
            else
            {
                monster.StateMachine.ChangeState(new MonsterIdleState(monster));
                //Debug.Log($"[{monster.name}] Suspicious → Idle 상태 전이");
            }
        }
    }

    public void Exit()
    {
        //Debug.Log($"[SuspiciousState] {monster.name} 수상 상태 종료");

        if (monster.Agent.isOnNavMesh)
            monster.Agent.isStopped = false;
    }
}