using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPacifiedState : IMonsterState
{
    private CatAI cat;
    private float timer;
    private float duration;

    public CatPacifiedState(float duration)
    {
        this.duration = duration;
    }

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        if (cat == null) return;

        timer = 0f;
        cat.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
        Debug.Log($"[{cat.name}] ����: CatPacified ���� (����ȭ {duration}��)");
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (cat.IsDead)
        {
            cat.StateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        if (timer >= duration)
        {
            Debug.Log($"[{cat.name}] ����ȭ ���� �� CatIdle ��ȯ");
            cat.StateMachine.ChangeState(new CatIdleState());
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] ����: CatPacified ����");
    }
}

