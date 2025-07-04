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
    [SerializeField] public float damage_Init;
    [SerializeField][Range(0.1f, 2)] public float mouseSensitivity_Init;



    public void Init()
    {
        base.SingletonInit();
        dm = DataManager.Instance;
    }

    private void OnDestroy()
    {

    }

    // => ���͸��� 0�� �Ǿ��� �� ȣ��
    public void PlayerFaint(float currentBattery)
    {
        if (currentBattery > 0) return;

        Debug.Log("������ ���� 9�÷� / �� ���� ���� ���� �ʱ�ȭ / �� �� �Ĺ� ������Ʈ ���� �ʱ�ȭ");

        // ������ ���� 9�÷� �̵�
        DailyManager.Instance.currentTimeData.CurrentDay.Value += 1;
        DailyManager.Instance.currentTimeData.CurrentTime = 0;
        DailyManager.Instance.currentTimeData.TZ_State.Value = TimeZoneState.Morning;

        // ���͸� �ִ뷮 ����
        instancePlayer.Status.Init_AfterFaint();

        // ���� & �Ĺֿ�����Ʈ �ʱ�ȭ
        StageManager.Instance.InitSpawnerRoutines();

        // ��ġ �̵�
        BaseCampManager.Instance.MoveToLastCamp(false);

        // �ڵ� ����
        dm.SaveData(0);
    }

    // => �Ӹ� �������� 0�� �Ǿ��� �� ȣ��
    public void PlayerDead()
    {
        // ������ ���̺� �����ͷ� �̵�(��¥, �ð� / �� ���� ���� & �Ĺ� ������Ʈ ���� �ʱ�ȭ / �÷��̾� ���� �ʱ�ȭ), ��ġ �̵�
        dm.LoadData(0);

        // ������ �ִ� ������ ��� ����
        instancePlayer.Status.Init_AfterDead();

        // �ֱ� �ӽ���Ʈ �ִٸ� �ı�
        if (BaseCampManager.Instance.currentTempCampInstance != null)
            BaseCampManager.Instance.currentTempCampInstance.DestroyWithData();

        // �ڵ� ����
        dm.SaveData(0);
    }


}
