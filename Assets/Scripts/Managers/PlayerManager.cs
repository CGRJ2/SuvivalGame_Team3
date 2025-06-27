using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �Ŵ������ ���� �ɵ�
public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerController instancePlayer;


    public void Init()
    {
        base.SingletonInit();
    }


    public void PlayerFaint() // => ���͸��� 0�� �Ǿ��� �� ȣ��
    {
        Debug.Log("�÷��̾� ���� => �ִ� ���͸��� ����");
        MoveToLastCamp();
    }

    public void PlayerDead() // => �Ӹ� �������� 0�� �Ǿ��� �� ȣ��
    {
        MoveToLastCamp();
    }

    // ���������� ����� ķ���� �̵�
    public void MoveToLastCamp()
    {
        Debug.Log("���������� ����� ķ���� �̵�");
    }

}
