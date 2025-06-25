using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPacifiedState : IMonsterState
{
    private BaseMonster monster;
    private float pacifyDuration;
    private float timer;

    public CatPacifiedState(float duration)
    {
        pacifyDuration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Idle); // ����ȭ �߿��� ��赵 �ʱ�ȭ or ����
        monster.GetComponent<MonsterView>()?.PlayMonsterPacifyAnimation();

        Debug.Log($"[{monster.name}] ����: CatPacified ���� ({pacifyDuration:F1}��)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        timer += Time.deltaTime;

        if (timer >= pacifyDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            monster.StateMachine.ChangeState(next);
            Debug.Log($"[{monster.name}] ����ȭ ���� �� {current} ���� ����");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ����: CatPacified ����");
    }
}

