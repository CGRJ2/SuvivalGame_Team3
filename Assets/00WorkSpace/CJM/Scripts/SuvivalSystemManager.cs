using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuvivalSystemManager : Singleton<SuvivalSystemManager>
{
    // ������ �ٸ� �Ŵ����麸�� �� �������� �ʱ�ȭ �ؾ� ��
    PlayerManager pm;

    [Header("���� ��ġ �Ҹ� �ֱ� ����")]
    [SerializeField] private float TickDuration;

    [System.Serializable]
    private class BatterySystem
    {
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
    private class WillPowerSystem
    {
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

    [Header("���͸� �ý���")]
    [SerializeField] BatterySystem batterySystem;

    [Header("���ŷ� �ý���")]
    [SerializeField] WillPowerSystem willPowerSystem;

    Coroutine coroutine_DecreaseWillPower;
    Coroutine coroutine_DecreaseBattery;

    // �׽�Ʈ�뵵////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DecreaseRoutineStart();
        }

    }
    ////////////////////////////////////////////////////////////////////////
    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }

    public void DecreaseRoutineStart()
    {
        coroutine_DecreaseWillPower = StartCoroutine(DecreaseWillPowerOverTime());
        coroutine_DecreaseBattery = StartCoroutine(DecreaseBatteryOverTime());
    }

    public void DecreaseRoutineStop()
    {
        if (coroutine_DecreaseWillPower != null) StopCoroutine(coroutine_DecreaseWillPower);
        if (coroutine_DecreaseBattery != null) StopCoroutine(coroutine_DecreaseBattery);
    }

    IEnumerator DecreaseWillPowerOverTime()
    {
        PlayerStatus ps = pm.instancePlayer.Status;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            if (false/*���̸�*/)
            {
                ps.WillPower.Value -= willPowerSystem.DrainPerTick_Night;
            }
            else
            {
                ps.WillPower.Value -= willPowerSystem.DrainPerTick_Idle;
            }
        }
    }

    IEnumerator DecreaseBatteryOverTime()
    {
        PlayerStatus ps = pm.instancePlayer.Status;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            // �޸��� ���¶��
            if (ps.IsCurrentState(PlayerStateTypes.Sprint))
            {
                ps.Battery.Value -= batterySystem.DrainPerTick_Sprint;
            }
            else
            {
                ps.Battery.Value -= batterySystem.DrainPerTick_Idle;
            }
        }
    }

}
