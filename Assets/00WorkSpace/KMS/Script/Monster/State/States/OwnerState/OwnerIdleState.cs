using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerIdleState : MonsterIdleState, IMonsterState
{
    public OwnerIdleState(BaseMonster monster) : base(monster) { }
    private BaseMonster monster;
    private float idleDuration;
    private float idleTimer;

    public override void Enter(BaseMonster monster)
    {
        base.Enter(monster);
        this.monster = monster;
        idleTimer = 0f;
        idleDuration = Random.Range(1f, 3f); // ��̶� ������ ������ ����.

        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

    }
    public override void Execute()
    {
        base.Execute();
        if (monster == null || monster.IsDead) return;

        idleTimer += Time.deltaTime;

        // �÷��̾ �����ϸ� ��赵 ���
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(40f);

        // �ð��� ������ ���� ���� �Ǵ�
        if (idleTimer >= idleDuration)
        {
            var current = monster.GetCurrentPerceptionState();
            var next = monster.StateFactory.GetStateForPerception(current);

            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
            }
            else
            {
                idleTimer = 0f;
                idleDuration = Random.Range(1f, 3f);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log($"[{monster.name}] OwnerIdle ����");
    }
}
