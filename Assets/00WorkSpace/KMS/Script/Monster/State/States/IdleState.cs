using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        // �ִϸ��̼� ���� ��
    }

    public void Execute()
    {
        // ���� Ž�� ���� ��
    }

    public void Exit()
    {
        // ������ �� ����
    }
}

