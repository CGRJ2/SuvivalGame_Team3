using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // �÷��̾� �ν��Ͻ��� �������� �����ϱ� ���� Ŭ���� 

    [HideInInspector] public PlayerController instancePlayer;

    [Header("��Ÿ�� �� �Һ� �� ����")] /////////////////////
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    public float MinPitch { get { return minPitch; } }
    public float MaxPitch { get { return maxPitch; } }
    [SerializeField] private float rotateSpeed_Init;
    [SerializeField] private float crouchSpeed_Init;
    [field: SerializeField] public float DamagedInvincibleTime { get; private set; }
    [field: SerializeField] public float AttackCoolTime { get; private set; }
    public float CrouchSpeed { get { return crouchSpeed_Init; } }
    public float RotateSpeed { get { return rotateSpeed_Init; } }
    //////////////////////////////////////////////////////

    [Header("�ʱⰪ ����")]////////////////////////
    [SerializeField] public float moveSpeed_Init;
    [SerializeField] public float sprintSpeed_Init;
    [SerializeField] public float jumpForce_Init;
    [SerializeField] public int damage_Init;
    [SerializeField][Range(0.1f, 2)] public float mouseSensitivity_Init;
    




    public void Init()
    {
        base.SingletonInit();
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
