using UnityEngine;

public class MonsterSuspiciousState : IMonsterState
{
    private BaseMonster monster;
    private float stateDuration = 4f;
    private float stateTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        stateTimer = 0f;

        Debug.Log($"[SuspiciousState] {monster.name} 수상 상태 진입");
        monster.SetPerceptionState(MonsterPerceptionState.Suspicious);
        monster.GetComponent<MonsterView>()?.PlayMonsterSuspiciousAnimation();
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        stateTimer += Time.deltaTime;

        // 플레이어 감지시 경계도 상승
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(5f);
        }

        // 대상 추적
        var target = monster.GetTarget();
        if (target != null)
        {
            Vector3 toTarget = target.position - monster.transform.position;
            toTarget.y = 0f;
            monster.MoveTo(toTarget.normalized * 0.7f); // Suspicious 상태는 느리게 접근
        }

        // 상태 전이/유지 분기
        if (stateTimer >= stateDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] Suspicious 종료 → {current} 상태 전이");
            }
            else
            {
                stateTimer = 0f;
                Debug.Log($"[{monster.name}] Suspicious 상태 유지");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[SuspiciousState] {monster.name} 수상 상태 종료");
    }
}