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
                if (pc.Status.GetPart(BodyPartTypes.Head).Hp - willPowerSystem.HeadDamagePerTick > 0)
                    pc.Status.GetPart(BodyPartTypes.Head).Hp -= willPowerSystem.HeadDamagePerTick;
                else pc.Status.GetPart(BodyPartTypes.Head).Hp = 0;
            }
        }
    }

}

/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////////
/// </summary>




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
    [field: Header("���͸� �ִ뷮 �ʱⰪ ����")]
    [field: SerializeField] public int MaxBattery_Init { get; private set; }


    [field: Header("���� �� ������ ���͸� �ִ뷮")]
    [field: SerializeField] public int MaxBattery_AfterFaint { get; private set; }

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


