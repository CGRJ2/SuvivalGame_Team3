using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerSleepState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        monster.StateMachine.ChangeState(new MonsterIdleState(monster)); // ���� �߿� ��� ��赵/���� off
        monster.GetComponent<MonsterView>()?.PlayMonsterSleepAnimation(); // ���� �ִϸ����Ϳ��� sleep Ʈ����
        Debug.Log($"[{monster.name}] Sleep ���� ����");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Sleep ���� ����");
    }
}
