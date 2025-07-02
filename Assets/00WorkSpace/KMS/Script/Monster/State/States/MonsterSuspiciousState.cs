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

        // �÷��̾� ������ ��赵 ���
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(5f);
        }

        // ��� ����
        var target = monster.GetTarget();
        if (target != null)
        {
            Vector3 toTarget = target.position - monster.transform.position;
            toTarget.y = 0f;
            monster.MoveTo(toTarget.normalized * 0.7f); // Suspicious ���´� ������ ����
        }

        // ���� ����/���� �б�
        if (stateTimer >= stateDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] Suspicious ���� �� {current} ���� ����");
            }
            else
            {
                stateTimer = 0f;
                Debug.Log($"[{monster.name}] Suspicious ���� ����");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[SuspiciousState] {monster.name} ���� ���� ����");
    }
}