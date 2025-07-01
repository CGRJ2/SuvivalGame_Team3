using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerIdleState : IMonsterState
{
    private BaseMonster monster;
    private float idleDuration;
    private float idleTimer;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        idleTimer = 0f;
        idleDuration = Random.Range(1f, 3f); // ��̶� ������ ������ ����.

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{monster.name}] ����: CatIdle ���� (��� {idleDuration:F1}s)");
    }
    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        idleTimer += Time.deltaTime;

        // �÷��̾ �����ϸ� ��赵 ���
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(15f); //�Ϲ� ���ͺ��� ���� ��ġ ���
        }

        // �ð��� ������ ���� ���� �Ǵ�
        if (idleTimer >= idleDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
                Debug.Log($"[{monster.name}] CatIdle �� {current} ���� ����");
            }
            else
            {
                // ������ Idle ���¶�� ���� Ÿ�̸� �缳��
                idleTimer = 0f;
                idleDuration = Random.Range(2f, 5f);
                Debug.Log($"[{monster.name}] CatIdle ���� ���� (�� Ÿ�̸�: {idleDuration:F1}s)");
            }
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] OwnerIdle ����");
    }
}
