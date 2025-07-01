using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerPacifiedState : IMonsterState
{
    private BaseMonster monster;
    private float duration;
    private float timer;

    public OwnerPacifiedState(float duration)
    {
        this.duration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        monster.GetComponent<MonsterView>()?.PlayMonsterPacifyAnimation(); // �ִϸ����� Ʈ����
        Debug.Log($"[{monster.name}] OwnerStun ���� (6��)");
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            // ���� ���� �Ǵ�
            var next = monster.GetCurrentPerceptionState() == MonsterPerceptionState.Alert
                ? monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert)
                : monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Idle);
            monster.StateMachine.ChangeState(next);
            Debug.Log($"[{monster.name}] ����ȭ ���� �� ���� ����");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] OwnerStun ����");
    }
}
