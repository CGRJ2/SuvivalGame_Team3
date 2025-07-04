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

        Debug.Log($"[SuspiciousState] {monster.name} ���� ���� ����");
        monster.SetPerceptionState(MonsterPerceptionState.Suspicious);
        monster.GetComponent<MonsterView>()?.PlayMonsterSuspiciousAnimation();
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        stateTimer += Time.deltaTime;

        // Suspicious ���¿����� ��赵�� ���������� ���
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(Time.deltaTime * 5f);

        // Suspicious �ּ� ���� �ð� ��� �Ŀ��� ���� ��
        if (stateTimer >= stateDuration)
        {
            // Perception ��(��赵+���� ��)
            MonsterPerceptionState nextPerception = monster.GetCurrentPerceptionState();

            if (nextPerception == MonsterPerceptionState.Alert)
            {
                monster.StateMachine.ChangeState(monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert));
                Debug.Log($"[{monster.name}] Suspicious �� Alert ���� ����");
            }
            else
            {
                monster.StateMachine.ChangeState(new MonsterIdleState(monster));
                Debug.Log($"[{monster.name}] Suspicious �� Idle ���� ����");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[SuspiciousState] {monster.name} ���� ���� ����");
    }
}