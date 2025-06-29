using System.Collections;
using UnityEngine;

public class SuvivalSystemManager : Singleton<SuvivalSystemManager>
{
    // ������ �ٸ� �Ŵ����麸�� �� �������� �ʱ�ȭ �ؾ� ��
    PlayerManager pm;
    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }
    ///////////////////////////////////////////

    [Header("���� ��ġ �Ҹ� �ֱ� ����")]
    [SerializeField] private float TickDuration;

    [System.Serializable]
    public class BodyPartSystem
    {
        [field: Header("�⺻ �ִ� ü�� �ʱⰪ ����")]
        [field: SerializeField] public int HeadMaxHP_Init { get; private set; }
        [field: SerializeField] public int ArmMaxHP_Init { get; private set; }
        [field: SerializeField] public int LegMaxHP_Init { get; private set; }


        [field: Header("�ı� ���¿��� 1�� ȸ�� ��� �� �ִ�ü�� ���� �� ����")]
        [field: SerializeField] public int HeadMaxHP_AfterDestroyed { get; private set; }
        [field: SerializeField] public int ArmMaxHP_AfterDestroyed { get; private set; }
        [field: SerializeField] public int LegMaxHP_AfterDestroyed { get; private set; }
    }

    [System.Serializable]
    public class BatterySystem
    {
        [field: Header("���ŷ� �ִ뷮 �ʱⰪ ����")]
        [field: SerializeField] public int MaxBattery_Init { get; private set; }

        [field: Header("���� �� ƽ�� ���ҷ�")]
        [field: SerializeField] public int DrainPerTick_Idle { get; private set; }
        [field: SerializeField] public int DrainPerTick_Sprint { get; private set; }


        [field: Header("Ư�� ���� �� ���͸� ȸ����")]
        [field: SerializeField] public int RecoverAmount_MonsterSlay { get; private set; }
        [field: SerializeField] public int RecoverAmount_BossHit { get; private set; }

        // ������ ������� ���� ȸ���� ������ Ŭ�������� ����
        // ���ͷ������� ���� ȸ���� ���ͷ��ͺ� ������Ʈ Ŭ�������� ����

        // ��� ��Ϳ��� ���͸� �Ҹ��� ������ ���⼭ ����
        // [SerializeField] private int decreaseAmout_Gimic; 

        // ��ͺ��� ���͸� �Ҹ��� ���� �ٸ��� �����ϰ� �ʹٸ� => ��� ��ü���� ���͸�����() ����
    }

    [System.Serializable]
    public class WillPowerSystem
    {
        [field: Header("���ŷ� �ִ뷮 �ʱⰪ ����")]
        [field: SerializeField] public int MaxWillPower_Init { get; private set; }


        [field: Header("���� �� ƽ�� ���ҷ�")]
        [field: SerializeField] public int DrainPerTick_Idle { get; private set; }
        [field: SerializeField] public int DrainPerTick_Night { get; private set; }
        [field: SerializeField] public int HeadDamagePerTick { get; private set; }

        [field: Header("�Ӹ��� �������� �ֱ� �����ϴ� ���ŷ� ��ġ ����")]
        [field: SerializeField] public int WillDangerZone { get; private set; }



        [field: Header("�̺�Ʈ �� ���ҷ�")]
        [field: SerializeField] public int DecreaseAmount_CatEvent { get; private set; }

        // DecreaseAmount_CustomMonster; ��ȹ���� �߰��� �� �߰�
    }

    [field: Header("������ ������ �ý���")]
    [field: SerializeField] public BodyPartSystem bodyPartSystem { get; private set; }

    [field: Header("���͸� �ý���")]
    [field: SerializeField] public BatterySystem batterySystem { get; private set; }

    [field: Header("���ŷ� �ý���")]
    [field: SerializeField] public WillPowerSystem willPowerSystem { get; private set; }

    Coroutine coroutine_DecreaseWillPower;
    Coroutine coroutine_DecreaseBattery;
    Coroutine coroutine_DotDamageByLowWill;

    // �׽�Ʈ�뵵////////////////////////////////////////////////////////////
    private void Start()
    {
        StartDecreaseRoutines();
    }
    ////////////////////////////////////////////////////////////////////////

    private void OnDestroy()
    {
        StopDecreaseRoutines();
    }

    public void StartDecreaseRoutines()
    {
        coroutine_DecreaseWillPower = StartCoroutine(DecreaseWillPowerOverTime());
        coroutine_DecreaseBattery = StartCoroutine(DecreaseBatteryOverTime());
        coroutine_DotDamageByLowWill = StartCoroutine(DotDamageByLowWillOverTime());
    }

    public void StopDecreaseRoutines()
    {
        if (coroutine_DecreaseWillPower != null) StopCoroutine(coroutine_DecreaseWillPower);
        if (coroutine_DecreaseBattery != null) StopCoroutine(coroutine_DecreaseBattery);
        if (coroutine_DotDamageByLowWill != null) StopCoroutine(coroutine_DotDamageByLowWill);
    }

    // ���ŷ� ���� ���� ��ƾ
    IEnumerator DecreaseWillPowerOverTime()
    {
        PlayerStatus ps = pm.instancePlayer.Status;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            if (Temp_DailyManager.Instance.TZ_State.Value == TimeZoneState.Night)
            {
                if (ps.CurrentWillPower.Value - willPowerSystem.DrainPerTick_Night > 0)
                    ps.CurrentWillPower.Value -= willPowerSystem.DrainPerTick_Night;
                else ps.CurrentWillPower.Value = 0;

            }
            else
            {
                if (ps.CurrentWillPower.Value - willPowerSystem.DrainPerTick_Idle > 0)
                    ps.CurrentWillPower.Value -= willPowerSystem.DrainPerTick_Idle;
                else ps.CurrentWillPower.Value = 0;
            }
        }
    }

    // ���͸� ���� ���� ��ƾ
    IEnumerator DecreaseBatteryOverTime()
    {
        PlayerStatus ps = pm.instancePlayer.Status;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            // �޸��� ���¶��
            if (ps.IsCurrentState(PlayerStateTypes.Sprint))
            {
                if (ps.CurrentBattery.Value - batterySystem.DrainPerTick_Sprint > 0)
                    ps.CurrentBattery.Value -= batterySystem.DrainPerTick_Sprint;
                else ps.CurrentBattery.Value = 0;
            }
            else
            {
                if (ps.CurrentBattery.Value - batterySystem.DrainPerTick_Idle > 0)
                    ps.CurrentBattery.Value -= batterySystem.DrainPerTick_Idle;
                else ps.CurrentBattery.Value = 0;
            }
        }
    }


    // ���ŷ��� ������ġ ������ �� �Ӹ� ���� ������ ��ƽ
    IEnumerator DotDamageByLowWillOverTime()
    {
        PlayerStatus ps = pm.instancePlayer.Status;
        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            if (pm.instancePlayer.Status.CurrentWillPower.Value <= willPowerSystem.WillDangerZone)
            {
                if (ps.GetPart(BodyPartTypes.Head).Hp.Value - willPowerSystem.HeadDamagePerTick > 0)
                    ps.GetPart(BodyPartTypes.Head).Hp.Value -= willPowerSystem.HeadDamagePerTick;
                else ps.GetPart(BodyPartTypes.Head).Hp.Value = 0;
            }
        }
    }


    // ���͸� ��ü (�ʱ�ȭ)
    public void InitBattery()
    {
        pm.instancePlayer.Status.InitBattery();
    }

    // ���͸� ȸ��
    public void RecoveryBattery(int amount)
    {
        pm.instancePlayer.Status.ChargeBattery(amount);
    }


    // ���� ��ü (�ʱ�ȭ)
    public void InitBodyPart(BodyPart bodyPart)
    {
        bodyPart.Init();
    }

    // ���� ȸ��
    public void RepairBodyPart(BodyPart bodyPart, int amount)
    {
        bodyPart.Repair(amount);
    }

    // ���� Ȱ��ȭ (���� ȸ��)
    public void QuickRepairBodyPart(BodyPart bodyPart)
    {
        if (bodyPart.type == BodyPartTypes.Head)
        {
            bodyPart.QuickRepair(bodyPartSystem.HeadMaxHP_AfterDestroyed);
        }
        else if (bodyPart.type == BodyPartTypes.RightArm || bodyPart.type == BodyPartTypes.LeftArm)
        {
            bodyPart.QuickRepair(bodyPartSystem.ArmMaxHP_AfterDestroyed);
        }
        else if (bodyPart.type == BodyPartTypes.RightLeg || bodyPart.type == BodyPartTypes.LeftLeg)
        {
            bodyPart.QuickRepair(bodyPartSystem.LegMaxHP_AfterDestroyed);
        }

    }



}
