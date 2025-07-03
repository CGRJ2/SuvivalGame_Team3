using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // �÷��̾� �ν��Ͻ��� �������� �����ϱ� ���� Ŭ���� 

    [HideInInspector] public PlayerController instancePlayer;


    public void Init()
    {
        base.SingletonInit();
    }

    public void SavePlayerData()
    {
        // �Ʒ� 2���� ������ ����
        // PlayerStatus status = instancePlayer.Status;
        // Transform playerTranform = instancePlayer.transform;
    }

    public void LoadPlayerData()
    {
        // �Ʒ� 2���� ������ �ҷ�����
        // PlayerStatus status = instancePlayer.Status;
        // Transform playerTranform = instancePlayer.transform;
    }


    // => ���͸��� 0�� �Ǿ��� �� ȣ��
    public void PlayerFaint() 
    {
        // ���͸� �ִ뷮 ����
        instancePlayer.Status.Init_AfterFaint(); 
        Debug.Log("������ ���� 9�÷� / �� ���� ���� ���� �ʱ�ȭ / �� �� �Ĺ� ������Ʈ ���� �ʱ�ȭ");

        // ��ġ �̵�
        MoveToLastCamp();
    }

    // => �Ӹ� �������� 0�� �Ǿ��� �� ȣ��
    public void PlayerDead() 
    {
        // ������ �ִ� ������ ��� ����
        instancePlayer.Status.Init_AfterDead(); 

        Debug.Log("������ ���̺� �����ͷ� �̵�(��¥, �ð� / �� ���� ���� ���� �ʱ�ȭ / �� �� �Ĺ� ������Ʈ ���� �ʱ�ȭ / �ڵ�����)");

        // ��ġ �̵�
        MoveToLastCamp();
    }

    // ���������� ����� ķ���� �̵�
    public void MoveToLastCamp()
    {
        BaseCampManager bcm = BaseCampManager.Instance;
        Debug.Log("���������� ����� ķ���� �̵�");

        // ���� ķ���� �ִٸ�
        if (bcm.tempCampData != null)
        {
            Debug.Log("����ķ���� �̵�");
            Debug.Log(bcm.tempCampData);
            instancePlayer.Respawn(bcm.tempCampData.respawnPoint);

            // ���� ķ������ ������ �Ǹ鼭 ����ķ�� ����
            bcm.currentTempCampInstance.DestroyTempCamp();
        }
        // ������ ���̽�ķ���� �̵�
        else
        {
            Debug.Log("���̽�ķ���� �̵�");
            instancePlayer.Respawn(bcm.baseCampInstance.GetRespawnPointTransform());
        }
    }

}
