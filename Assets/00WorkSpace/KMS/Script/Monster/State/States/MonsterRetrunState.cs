using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnState : IMonsterState
{
    private BaseMonster monster;

    private float returnTime = 0f;
    private const float minReturnTime = 2f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;

        monster.SetPerceptionState(MonsterPerceptionState.Idle); // ���ư��� ���� ���� �� �ϰ� �Ϸ���
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();

        Debug.Log($"[{monster.name}] ����: Return ���� - ������������ ���� ����");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        returnTime += Time.deltaTime;

        monster.DecreaseAlert(50f * Time.deltaTime); //�ʴ� 50����

        Vector3 toSpawn = monster.GetSpawnPoint() - monster.transform.position;
        toSpawn.y = 0f;

        if (toSpawn.magnitude > 0.5f)
        {
            monster.Move(toSpawn.normalized);
        }
        else
        {
            if (returnTime < minReturnTime)
            {
                // ���������� 2�� ä�� ������ ���
                monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
                return;
            }

            Debug.Log($"[{monster.name}] �������� ���� + ����ȭ �Ϸ� �� Idle ���� ��ȯ");
            monster.StateMachine.ChangeState(monster.GetIdleState());
        }
    }
    public void Exit()
    {
        Debug.Log($"[{monster.name}] Return ���� �� ���� �ʱ�ȭ");

        monster.ResetAlert();

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
    }
    // TODO: ���� ���� �� �÷��̾� ���� �ߴ� ����� �۵��ϴ��� ����
}
