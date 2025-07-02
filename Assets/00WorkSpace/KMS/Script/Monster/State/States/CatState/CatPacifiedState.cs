using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPacifiedState : IMonsterState
{
    private CatAI cat;
    private float pacifyDuration;
    private float timer;

    public CatPacifiedState(float duration)
    {
        pacifyDuration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        timer = 0f;

        // ����ȭ ���¿����� ��赵/�ۼ��� ��� Idle�� ����
        cat.SetPerceptionState(MonsterPerceptionState.Idle);
        cat.GetComponent<MonsterView>()?.PlayMonsterPacifyAnimation();

        Debug.Log($"[{cat.name}] ����: CatPacified ���� ({pacifyDuration:F1}��)");
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead) return;

        timer += Time.deltaTime;

        if (timer >= pacifyDuration)
        {
            // ����ȭ ���� ���� �� �� �ۼ��� ���� ������� ��������
            var next = cat.StateFactory.GetStateForPerception(cat.GetCurrentPerceptionState());

            cat.StateMachine.ChangeState(next);
            Debug.Log($"[{cat.name}] ����ȭ ���� �� {next.GetType().Name} ���� ����");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] ����: CatPacified ����");
    }
}