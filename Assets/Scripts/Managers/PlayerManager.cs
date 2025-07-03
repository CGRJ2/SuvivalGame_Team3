using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // �÷��̾� �ν��Ͻ��� �������� �����ϱ� ���� Ŭ���� 

    [HideInInspector] public PlayerController instancePlayer;
    DataManager dm;


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
        dm = DataManager.Instance;
    }


    // => ���͸��� 0�� �Ǿ��� �� ȣ��
    public void PlayerFaint(SaveDataGroup saveDataGroup) 
    {
        Debug.Log("������ ���� 9�÷� / �� ���� ���� ���� �ʱ�ȭ / �� �� �Ĺ� ������Ʈ ���� �ʱ�ȭ");

        // ������ ���� 9�÷� �̵�
        DailyManager.Instance.currentTimeData.CurrentDay.Value += 1;
        DailyManager.Instance.currentTimeData.CurrentTime = 0;
        DailyManager.Instance.currentTimeData.TZ_State.Value = TimeZoneState.Morning;

        // ���͸� �ִ뷮 ����
        instancePlayer.Status.Init_AfterFaint(); 

        // ��ġ �̵�
        MoveToLastCamp();

        // �ڵ� ����
        //dm.SaveData(0);
    }

    // => �Ӹ� �������� 0�� �Ǿ��� �� ȣ��
    public void PlayerDead() 
    {
        //������ ���̺� �����ͷ� �̵�(��¥, �ð� / �� ���� ���� & �Ĺ� ������Ʈ ���� �ʱ�ȭ / �÷��̾� ���� �ʱ�ȭ)
        dm.LoadData(0);

        // ������ �ִ� ������ ��� ����
        instancePlayer.Status.Init_AfterDead(); 

        // ��ġ �̵�
        MoveToLastCamp();

        // �ڵ� ����
        dm.SaveData(0);

    }

    // ���������� ����� ķ���� �̵�
    public void MoveToLastCamp()
    {
        BaseCampManager bcm = BaseCampManager.Instance;
        Debug.Log("���������� ����� ķ���� �̵�");

        // ���� ķ�� �����Ͱ� �ִٸ� 
        if (bcm.tempCampData != null)
        {
            Debug.Log("����ķ���� �̵�");
            instancePlayer.Respawn(bcm.tempCampData.respawnPoint);

            // �ν��Ͻ��� �ִٸ� 
            if (bcm.currentTempCampInstance != null)
            {
                // ����ķ�� �Ҹ�(�ı�)
                bcm.currentTempCampInstance.DestroyTempCamp();
            }
            // �ν��Ͻ��� ���ٸ� 
            else
            {
                // ���� ������ ����Ʈ(������ ����� ���� ķ�� ��ġ)�� ���� ķ�� ������ ��ȯ
                GameObject tempCampInstance = bcm.SpawnTempBaseCampInstance(bcm.tempCampData.respawnPoint);
                TemporaryCampInstance temp = tempCampInstance.GetComponent<TemporaryCampInstance>();
                
                // ����ķ�� �Ҹ�(�ı�)
                if (temp != null) temp.DestroyTempCamp();
            }

            // ���� ķ������ ������ �Ǹ鼭 ����ķ�� ����
            bcm.tempCampData = null;
        }
        // ������ ���̽�ķ���� �̵�
        else
        {
            Debug.Log("���̽�ķ���� �̵�");
            instancePlayer.Respawn(bcm.baseCampInstance.GetRespawnPointTransform());
        }
    }

}
