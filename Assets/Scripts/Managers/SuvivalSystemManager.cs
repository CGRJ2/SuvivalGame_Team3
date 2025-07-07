using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuvivalSystemManager : Singleton<SuvivalSystemManager>
{
    ///////////////////////////////////////////

    [Header("���� ��ġ �Ҹ� �ֱ� ����")]
    [SerializeField] private float TickDuration;

    [field: Header("������ ������ �ý���")]
    [field: SerializeField] public BodyPartSystem bodyPartSystem { get; private set; }

    [field: Header("���͸� �ý���")]
    [field: SerializeField] public BatterySystem batterySystem { get; private set; }

    [field: Header("���ŷ� �ý���")]
    [field: SerializeField] public WillPowerSystem willPowerSystem { get; private set; }

    //[Header("�Ĺ� �ý���")]
    //public FarmingSystem farmingSystem;

    Coroutine coroutine_DecreaseWillPower;
    Coroutine coroutine_DecreaseBattery;
    Coroutine coroutine_DotDamageByLowWill;

    public void Init()
    {
        base.SingletonInit();

    }

    

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
        PlayerController pc = PlayerManager.Instance.instancePlayer;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            if (DailyManager.Instance.currentTimeData.TZ_State.Value == TimeZoneState.Night)
            {
                if (pc.Status.CurrentWillPower.Value - willPowerSystem.DrainPerTick_Night > 0)
                    pc.Status.CurrentWillPower.Value -= willPowerSystem.DrainPerTick_Night;
                else pc.Status.CurrentWillPower.Value = 0;

            }
            else
            {
                if (pc.Status.CurrentWillPower.Value - willPowerSystem.DrainPerTick_Idle > 0)
                    pc.Status.CurrentWillPower.Value -= willPowerSystem.DrainPerTick_Idle;
                else pc.Status.CurrentWillPower.Value = 0;
            }
        }
    }

    // ���͸� ���� ���� ��ƾ
    IEnumerator DecreaseBatteryOverTime()
    {
        PlayerController pc = PlayerManager.Instance.instancePlayer;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            // �޸��� ���¶��
            if (pc.IsCurrentState(PlayerStateTypes.Sprint))
            {
                if (pc.Status.CurrentBattery.Value - batterySystem.DrainPerTick_Sprint > 0)
                    pc.Status.CurrentBattery.Value -= batterySystem.DrainPerTick_Sprint;
                else pc.Status.CurrentBattery.Value = 0;
            }
            else
            {
                if (pc.Status.CurrentBattery.Value - batterySystem.DrainPerTick_Idle > 0)
                    pc.Status.CurrentBattery.Value -= batterySystem.DrainPerTick_Idle;
                else pc.Status.CurrentBattery.Value = 0;
            }
        }
    }

    // ���ŷ��� ������ġ ������ �� �Ӹ� ���� ������ ��ƽ
    IEnumerator DotDamageByLowWillOverTime()
    {
        PlayerController pc = PlayerManager.Instance.instancePlayer;
        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            if (pc.Status.CurrentWillPower.Value <= willPowerSystem.WillDangerZone)
            {
                if (pc.Status.GetPart(BodyPartTypes.Head).Hp.Value - willPowerSystem.HeadDamagePerTick > 0)
                    pc.Status.GetPart(BodyPartTypes.Head).Hp.Value -= willPowerSystem.HeadDamagePerTick;
                else pc.Status.GetPart(BodyPartTypes.Head).Hp.Value = 0;
            }
        }
    }


    public float GetInitBodyPartsHPSum()
    {
        return bodyPartSystem.HeadMaxHP_Init + 2 * bodyPartSystem.ArmMaxHP_Init + 2 * bodyPartSystem.LegMaxHP_Init;
    }
}

/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////////
/// </summary>




[System.Serializable]
public class BodyPartSystem
{
    [field: Header("�⺻ �ִ� ������ �ʱⰪ ����")]
    [field: SerializeField] public float HeadMaxHP_Init { get; private set; }
    [field: SerializeField] public float ArmMaxHP_Init { get; private set; }
    [field: SerializeField] public float LegMaxHP_Init { get; private set; }

    [field: Header("�ִ� ������ �ּҰ� ����(����� �ݺ��ص� ����޴� �ּҰ�)")]
    [field: SerializeField] public float MinHeadMaxHPLimit { get; private set; }
    [field: SerializeField] public float MinArmMaxHPLimit { get; private set; }
    [field: SerializeField] public float MinLegMaxHPLimit { get; private set; }


    [field: Header("���� �ı� �� ���ҵǴ� �ִ� ������ ��")]
    [field: SerializeField] public float HeadMaxHPReduce_AD { get; private set; }
    [field: SerializeField] public float ArmMaxHPReduce_AD { get; private set; }
    [field: SerializeField] public float LegMaxHPReduce_AD { get; private set; }

    
}

[System.Serializable]
public class BatterySystem
{
    [field: Header("���͸� �ִ뷮 �ʱⰪ ����")]
    [field: SerializeField] public float InitMaxBattery { get; private set; }

    [field: Header("���͸� �ִ뷮 �ּҰ� ����(������ �ݺ��ص� ����޴� �ּڰ�)")]
    [field: SerializeField] public float MinBatteryLimit { get; private set; }

    [field: Header("���� �� ���͸� ���ҷ�")]
    [field: SerializeField] public float MaxBatteryReduceAfterFaint { get; private set; }

    [field: Header("���� �� ƽ�� ���ҷ�")]
    [field: SerializeField] public float DrainPerTick_Idle { get; private set; }
    [field: SerializeField] public float DrainPerTick_Sprint { get; private set; }


    [field: Header("Ư�� ���� �� ���͸� ȸ����")]
    [field: SerializeField] public float RecoverAmount_MonsterSlay { get; private set; }
    [field: SerializeField] public float RecoverAmount_BossHit { get; private set; }

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
    [field: SerializeField] public float MaxWillPower_Init { get; private set; }


    [field: Header("���� �� ƽ�� ���ҷ�")]
    [field: SerializeField] public float DrainPerTick_Idle { get; private set; }
    [field: SerializeField] public float DrainPerTick_Night { get; private set; }
    [field: SerializeField] public float HeadDamagePerTick { get; private set; }

    [field: Header("�Ӹ��� �������� �ֱ� �����ϴ� ���ŷ� ��ġ ����")]
    [field: SerializeField] public float WillDangerZone { get; private set; }



    [field: Header("�̺�Ʈ �� ���ҷ�")]
    [field: SerializeField] public float DecreaseAmount_CatEvent { get; private set; }

    // DecreaseAmount_CustomMonster; ��ȹ���� �߰��� �� �߰�
}


