using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : IMonsterState
{
    private BaseMonster monster;
    private bool animationPlayed = false;

    public void Enter(BaseMonster monster)
    {
        Debug.Log($"[{monster.name}] ����: Dead ����");
        this.monster = monster;
        animationPlayed = false;
        monster.Agent.enabled = false;
        if (monster.IsDead)
        {
            monster.GetComponent<MonsterView>()?.PlayMonsterDeathAnimation();
            animationPlayed = true;
            foreach (var rigid in monster.GetComponentsInChildren<Rigidbody>())
            {
                rigid.isKinematic = true;
                rigid.detectCollisions = false;
            }
        }
        else
        {
            Debug.LogWarning($"[{monster.name}] Dead ���� ���� ��û �� �׷��� isDead == false");
        }
        PlayerManager.Instance.instancePlayer.Status.ChargeBattery(SuvivalSystemManager.Instance.batterySystem.RecoverAmount_MonsterSlay);


        monster.view.Animator.SetBool("IsMove", false);

        monster.Agent.isStopped = true;
    }

    public void Execute()
    {
        // ��� ���¿����� �ƹ��͵� ���� ����
        // ���� �ִϸ��̼� �Ϸ� �� �ı� �� Ÿ�̹� ��� ó�� �ʿ� �� ���⿡ �ۼ�
    }

    public void Exit()
    {
        // ���� ���´� ������� �����Ƿ� Ư���� ó�� ����
        Debug.Log($"[{monster.name}] Dead ���¿��� Exit ȣ�� (������ ��Ȳ�� �� ����)");
        monster.view.Animator.SetBool("IsMove", true);
        if (monster.Agent.isOnNavMesh)
            monster.Agent.isStopped = false;
    }
}