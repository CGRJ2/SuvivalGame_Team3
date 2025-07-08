using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerState_SleepingSupicious : IMonsterState
{
    private BaseMonster monster;

    // �ʵ忡�� ������ �� �ְ� ���� �ʿ�
    private float stateDuration = 3f;
    private float stateTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        stateTimer = 0f;

        //Debug.Log($"[Sleeping SuspiciousState] {monster.name} ���� �� ���� ���� ����");
        monster.SetPerceptionState(MonsterPerceptionState.Suspicious);
        //monster.GetComponent<MonsterView>()?.PlayMonsterSuspiciousAnimation();
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        stateTimer += Time.deltaTime;

        // Suspicious ���¿����� ��赵�� ���������� ���
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(Time.deltaTime * 15f);

        // Suspicious �ּ� ���� �ð� ��� �Ŀ��� ���� ��
        if (stateTimer >= stateDuration)
        {
            // Perception ��(��赵+���� ��)
            MonsterPerceptionState nextPerception = monster.GetCurrentPerceptionState();

            if (nextPerception == MonsterPerceptionState.Alert)
            {
                monster.StateMachine.ChangeState(monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert));
                Debug.Log($"[{monster.name}] Sleeping Suspicious �� Alert ���� ����");
            }
            else
            {
                stateTimer = 0;
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[SuspiciousState] {monster.name} ���� ���� ����");
    }
}
