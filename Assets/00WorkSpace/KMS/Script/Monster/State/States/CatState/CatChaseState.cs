using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatChaseState : IMonsterState
{
    private CatAI cat;
    private Transform target;

    // ���ŷ� ���� �ֱ� �� ��ġ (�⺻��, ���߿� Ʃ�� ����)
    private float mentalTickInterval = 1.0f;
    private float mentalDamage = 3.0f;
    private float mentalTickTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        if (cat == null) return;

        target = cat.GetTarget();
        cat.GetComponent<MonsterView>()?.PlayRunAnimation();
        Debug.Log($"[{cat.name}] ����: CatChase ����");

        mentalTickTimer = 0f; // ���� �� �ʱ�ȭ
    }

    public void Execute()
    {
        if (target == null)
        {
            cat.StateMachine.ChangeState(new CatIdleState());
            return;
        }

        Vector3 dir = (target.position - cat.transform.position).normalized;
        cat.Move(dir);

        // ���ŷ� ���� ó��
        mentalTickTimer += Time.deltaTime;
        if (mentalTickTimer >= mentalTickInterval)
        {
            var mental = target.GetComponent<IMentalStamina>();
            if (mental != null)
            {
                mental.ReduceMental(mentalDamage);
                Debug.Log($"[{cat.name}] �� ���ŷ� {mentalDamage} ���� (���� ����)");
            }
            mentalTickTimer = 0f;
        }

        // ���� �ִϸ��̼��� �ܼ� �����
        float dist = Vector3.Distance(cat.transform.position, target.position);
        if (dist < 2f)
        {
            cat.GetComponent<MonsterView>()?.PlayAttackAnimation();
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] CatChase ����");
    }
}
