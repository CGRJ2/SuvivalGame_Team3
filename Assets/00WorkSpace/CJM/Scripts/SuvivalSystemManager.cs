using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuvivalSystemManager : Singleton<SuvivalSystemManager>
{
    // 참조한 다른 매니저들보다 후 순위에서 초기화 해야 함
    PlayerManager pm;

    [Header("생존 수치 소모 주기 설정")]
    [SerializeField] private float TickDuration;

    [System.Serializable]
    private class BatterySystem
    {
        [field: Header("상태 별 틱당 감소량")]
        [field: SerializeField] public int DrainPerTick_Idle { get; private set; }
        [field: SerializeField] public int DrainPerTick_Sprint { get; private set; }


        [field: Header("특정 조건 별 배터리 회복량")]
        [field: SerializeField] public int RecoverAmount_MonsterSlay { get; private set; }
        [field: SerializeField] public int RecoverAmount_BossHit { get; private set; }

        // 아이템 사용으로 인한 회복은 아이템 클래스에서 진행
        // 인터랙션으로 인한 회복은 인터랙터블 오브젝트 클래스에서 진행

        // 모든 기믹에서 배터리 소모량이 같으면 여기서 진행
        // [SerializeField] private int decreaseAmout_Gimic; 

        // 기믹별로 배터리 소모량을 각기 다르게 설정하고 싶다면 => 기믹 자체에서 배터리감소() 실행
    }

    [System.Serializable]
    private class WillPowerSystem
    {
        [field: Header("상태 별 틱당 감소량")]
        [field: SerializeField] public int DrainPerTick_Idle { get; private set; }
        [field: SerializeField] public int DrainPerTick_Night { get; private set; }
        [field: SerializeField] public int HeadDamagePerTick { get; private set; }

        [field: Header("머리에 내구도를 주기 시작하는 정신력 수치 기준")]
        [field: SerializeField] public int WillDangerZone { get; private set; }



        [field: Header("이벤트 별 감소량")]
        [field: SerializeField] public int DecreaseAmount_CatEvent { get; private set; }

        // DecreaseAmount_CustomMonster; 기획에서 추가될 시 추가
    }

    [Header("배터리 시스템")]
    [SerializeField] BatterySystem batterySystem;

    [Header("정신력 시스템")]
    [SerializeField] WillPowerSystem willPowerSystem;

    Coroutine coroutine_DecreaseWillPower;
    Coroutine coroutine_DecreaseBattery;

    // 테스트용도////////////////////////////////////////////////////////////
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

            if (false/*밤이면*/)
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

            // 달리기 상태라면
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
