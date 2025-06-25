using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterIdleState : IMonsterState
{
    private BaseMonster monster;
    private float timer;
    private float direction = 1f;
    private float lookDuration = 2f;
    private float rotateSpeed = 30f;
    private Quaternion baseRotation;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        timer = 0f;
        direction = 1f;
        lookDuration = Random.Range(1.5f, 3f);

        baseRotation = monster.transform.rotation;

        monster.SetPerceptionState(MonsterPerceptionState.Idle);
        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();

        Debug.Log($"[{monster.name}] ����: Idle ���� (�¿� ȸ��)");
    }

    public void Execute()
    {
        monster.transform.GetChild(0).Rotate(0f, 30f * Time.deltaTime, 0f);

        if (monster == null || monster.IsDead) return;

        // �÷��̾� ����
        if (monster.checkTargetVisible)
        {
            monster.IncreaseAlert(10f);
        }

        timer += Time.deltaTime;

        // ȸ��: ���ؿ��� �¿�� +-20�� ���̷� �պ� ȸ��
        float angleOffset = Mathf.Sin(timer * Mathf.PI / lookDuration) * 20f;
        Quaternion targetRotation = baseRotation * Quaternion.Euler(0f, angleOffset, 0f);
        monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, targetRotation, Time.deltaTime * 3f);

        // ���� �ð� �� ���� ������
        if (timer >= lookDuration * 2f)
        {
            timer = 0f;
            lookDuration = Random.Range(1.5f, 3f);
            Debug.Log($"[{monster.name}] �¿� ȸ�� ���� �ݺ�");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] ����: Idle ���� (ȸ�� �ʱ�ȭ)");
    }
}
