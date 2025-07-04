using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerAttackState : IMonsterState
{
    private BaseMonster monster;
    private Transform target;
    private float attackTimer = 0f;
    private bool hasThrown = false;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        this.target = monster.GetTarget();
        hasThrown = false;
        attackTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        monster.GetComponent<MonsterView>()?.PlayMonsterGrabThrowAnimation(); // �ִϸ��̼� ����

        Debug.Log($"[{monster.name}] ����: OwnerAttack ���� (��� ����)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        attackTimer += Time.deltaTime;

        // ����: 1�� �Ŀ� ������ �̺�Ʈ �߻�
        if (!hasThrown && attackTimer >= 1.0f)
        {
            ThrowTarget();
            hasThrown = true;
        }

        // ���� �� ���� �ð� �� Idle ����
        if (attackTimer >= 2.0f)
        {
            monster.StateMachine.ChangeState(new MonsterIdleState(monster));
        }
    }

    private void ThrowTarget()
    {
        if (target == null) return;

        Vector3 throwDirection = monster.transform.forward + Random.insideUnitSphere * 0.3f;
        throwDirection.y = 0.5f; // ���� ���� �߰�

        // �÷��̾ IThrowable�� �����Ǿ� ���� ���
       IThrowable throwable = target.GetComponent<IThrowable>();
       if (throwable != null)
       {
           float throwForce = 7f; // ���� �� (��ȹ�� ���� ����)
           throwable.ApplyThrow(throwDirection.normalized, throwForce);
           Debug.Log($"[{monster.name}] �÷��̾ ���� �� ����: {throwDirection.normalized}, ��: {throwForce}");
       }
       else
       {
           Debug.LogWarning($"[{monster.name}] ��� IThrowable �̱���");
       }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] OwnerAttack ����");
    }
}