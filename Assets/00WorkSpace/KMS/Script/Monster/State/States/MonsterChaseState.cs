using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : IMonsterState
{
    private BaseMonster monster;
    private Transform target;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        target = monster.GetTarget();
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();
        Debug.Log($"[{monster.name}] ����: Chase ����");
    }

    public void Execute()
    {
        if (target == null)
        {
            Debug.LogWarning("Ÿ���� �����ϴ�. Idle�� ����");
            return;
        }

        Vector3 dir = (target.position - monster.transform.position).normalized;
        monster.Move(dir);

        float dist = Vector3.Distance(monster.transform.position, target.position);
        if (dist < 2f) // ���� ���� �Ÿ� ����
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Chase ����");
    }
}
