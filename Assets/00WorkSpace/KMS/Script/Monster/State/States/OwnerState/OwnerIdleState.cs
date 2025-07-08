using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerIdleState : MonsterIdleState, IMonsterState
{
    public OwnerIdleState(BaseMonster monster) : base(monster) { }
    private float idleDuration;
    private float idleTimer;

    public override void Enter(BaseMonster monster)
    {
        base.Enter(monster);
        //this.monster = monster;
        //idleTimer = 0f;
        //idleDuration = Random.Range(3f, 5f); // ��̶� ������ ������ ����.

        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
        //Debug.Log($"[{monster.name}] ����: OwnerIdle ���� (����)");
    }
    public override void Execute()
    {
        base.Execute();
        //if (monster == null || monster.IsDead) return;

        /*idleTimer += Time.deltaTime;

        // �÷��̾ �����ϸ� ��赵 ���
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(40f);

        // �ð��� ������ ���� ���� �Ǵ�
        if (idleTimer >= idleDuration)
        {
            MonsterPerceptionState current = monster.GetCurrentPerceptionState();
            IMonsterState next = monster.StateFactory.GetStateForPerception(current);
            Debug.Log(next);
            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
            }
            else
            {
                idleTimer = 0f;
                idleDuration = Random.Range(1f, 3f);
            }
        }*/
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log($"[{monster.name}] OwnerIdle ����");
    }
}
