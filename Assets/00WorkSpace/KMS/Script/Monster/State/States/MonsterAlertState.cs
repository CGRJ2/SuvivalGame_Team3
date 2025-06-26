using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlertState : IMonsterState
{
    private BaseMonster monster;
    private float alertTimer = 0f;
    private float returnTimer = 0f;
    private float returnDelay = 3f;
    private float maxAlertDuration = 6f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        alertTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{monster.name}] ����: Alert ����");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead)
        {
            monster.StateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        // �ൿ �ݰ� ����� ���� �ߴ� + ���
        if (monster.IsOutsideActionRadius())
        {
            returnTimer += Time.deltaTime;

            if (returnTimer >= returnDelay)
            {
                Debug.Log($"[{monster.name}] Ȱ�� �ݰ� ��Ż 3�� �ʰ� �� Return ���� ��ȯ");
                monster.StateMachine.ChangeState(new MonsterReturnState());
                return;
            }

            monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
            return;
        }
        else
        {
            returnTimer = 0f;
        }

        // ���� �� ��赵 ���
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(15f);
        }

        // Ÿ�� ����
        var target = monster.GetTarget();
        if (target != null)
        {
            Vector3 toTarget = target.position - monster.transform.position;
            toTarget.y = 0f;
            monster.Move(toTarget.normalized);

            Debug.Log($"[AlertState] Ÿ�� ��ġ = {target.position}");
        }
        else
        {
            Debug.LogWarning("[AlertState] Ÿ���� �����ϴ�!");
        }

        // ���� ��Ÿ� ����
        if (monster.IsInAttackRange())
        {
            var attackState = monster.CreateAttackState();
            monster.StateMachine.ChangeState(attackState);
            return;
        }

        // ���� ����
        alertTimer += Time.deltaTime;
        if (alertTimer >= maxAlertDuration)
        {
            var nextState = monster.StateFactory.GetStateForPerception(monster.GetCurrentPerceptionState());
            if (nextState != this)
            {
                monster.StateMachine.ChangeState(nextState);
                Debug.Log($"[{monster.name}] Alert �ð� ���� �� {monster.GetCurrentPerceptionState()} ����");
            }
            else
            {
                alertTimer = 0f;
                Debug.Log($"[{monster.name}] Alert ���� ����");
            }
        }

        // ��赵 �϶� �� Idle ����
        if (monster.AlertLevel < monster.AlertThreshold_Low)
        {
            monster.SetPerceptionState(MonsterPerceptionState.Idle);
            var idleState = monster.StateFactory.GetStateForPerception(MonsterPerceptionState.Idle);
            monster.StateMachine.ChangeState(idleState);
            Debug.Log($"[{monster.name}] ��赵 �϶� �� Idle ����");
            return;
        }
    }
    public void Exit()
    {
        Debug.Log($"[{monster.name}] ����: Alert ����");
    }
}
