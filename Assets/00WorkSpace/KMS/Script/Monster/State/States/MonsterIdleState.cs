using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterIdleState : IMonsterState
{
    private BaseMonster monster;
    private float idleTime;
    private float timer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        idleTime = Random.Range(1f, 3f); // ���� ���

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{monster.name}] ����: Idle ���� (��� {idleTime:F1}s)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // �÷��̾ ���̸� ��赵 ���
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f);
        }

        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            var currentState = monster.GetCurrentPerceptionState();
            var nextState = monster.StateFactory.GetStateForPerception(currentState);

            // ���� Idle �����ε� ���� ��ȭ�� �����Ǿ��� ��� ����
            if (nextState != this)
            {
                monster.StateMachine.ChangeState(nextState);
                Debug.Log($"[{monster.name}] Idle �ð� ��� �� {currentState} ���� ����");
            }
            else
            {
                // �緣�� Ÿ�̸� ����
                timer = 0f;
                idleTime = Random.Range(1f, 3f);
                Debug.Log($"[{monster.name}] Idle ���� (�� idleTime: {idleTime:F1}s)");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ����: Idle ����");
    }
}


