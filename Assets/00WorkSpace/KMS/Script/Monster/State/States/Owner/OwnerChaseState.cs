using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerChaseState : IMonsterState
{
    private BaseMonster monster;
    private Transform target;
    private float lostTimer = 0f;
    private const float chaseLoseDelay = 2.5f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        target = monster.GetTarget();

        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{monster.name}] ����: OwnerChase ����");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        // �þ߿� Ÿ���� ���� Ž�� �ݰ� ���� ��� Ż�� ī��Ʈ ����
        if (!monster.checkTargetVisible || monster.IsOutsideDetectionRadius())
        {
            lostTimer += Time.deltaTime;

            if (lostTimer >= chaseLoseDelay)
            {
                Debug.Log($"[{monster.name}] ���� ���� => Idle ���� ����");
                monster.StateMachine.ChangeState(monster.GetIdleState());
            }

            return;
        }

        lostTimer = 0f; // ���� ���� �� �ʱ�ȭ

        Vector3 toPlayer = target.position - monster.transform.position;
        toPlayer.y = 0;

        monster.Move(toPlayer.normalized);

        // ���� ���� ���� �� ���� ����
       //if (monster.IsInAttackRange())
       //{
       //    Debug.Log($"[{monster.name}] �÷��̾� ���� => ��� ���� ���� ����");
       //    monster.StateMachine.ChangeState(monster.GetAttackState());
       //}
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] OwnerChase ����");
    }
}
