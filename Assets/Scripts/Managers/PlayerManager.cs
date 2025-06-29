using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // �÷��̾� �ν��Ͻ��� �������� �����ϱ� ���� Ŭ���� 

    public PlayerController instancePlayer;


    public void Init()
    {
        base.SingletonInit();
    }

    // => ���͸��� 0�� �Ǿ��� �� ȣ��
    public void PlayerFaint() 
    {
        Debug.Log("�÷��̾� ���� => �ִ� ���͸��� ����");
        MoveToLastCamp();
    }

    // => �Ӹ� �������� 0�� �Ǿ��� �� ȣ��
    public void PlayerDead() 
    {
        Debug.Log("�÷��̾� ���!");
        MoveToLastCamp();
    }

    // ���������� ����� ķ���� �̵�
    public void MoveToLastCamp()
    {
        Debug.Log("���������� ����� ķ���� �̵�");
    }

}
