using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuvivalSystemManager : Singleton<SuvivalSystemManager>
{
    ///////////////////////////////////////////

    [Header("생존 수치 소모 주기 설정")]
    [SerializeField] private float TickDuration;

    [field: Header("부위별 내구도 시스템")]
    [field: SerializeField] public BodyPartSystem bodyPartSystem { get; private set; }

    [field: Header("배터리 시스템")]
    [field: SerializeField] public BatterySystem batterySystem { get; private set; }

    [field: Header("정신력 시스템")]
    [field: SerializeField] public WillPowerSystem willPowerSystem { get; private set; }

    //[Header("파밍 시스템")]
    //public FarmingSystem farmingSystem;

    Coroutine coroutine_DecreaseWillPower;
    Coroutine coroutine_DecreaseBattery;
    Coroutine coroutine_DotDamageByLowWill;

    public void Init()
    {
        base.SingletonInit();

    }

    

    // 테스트용도////////////////////////////////////////////////////////////
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

    // 정신력 지속 감소 루틴
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

    // 배터리 지속 감소 루틴
    IEnumerator DecreaseBatteryOverTime()
    {
        PlayerController pc = PlayerManager.Instance.instancePlayer;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            // 달리기 상태라면
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

    // 정신력이 일정수치 이하일 때 머리 지속 데미지 루틱
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
    [field: Header("기본 최대 내구도 초기값 설정")]
    [field: SerializeField] public float HeadMaxHP_Init { get; private set; }
    [field: SerializeField] public float ArmMaxHP_Init { get; private set; }
    [field: SerializeField] public float LegMaxHP_Init { get; private set; }

    [field: Header("최대 내구도 최소값 제한(사망을 반복해도 보장받는 최소값)")]
    [field: SerializeField] public float MinHeadMaxHPLimit { get; private set; }
    [field: SerializeField] public float MinArmMaxHPLimit { get; private set; }
    [field: SerializeField] public float MinLegMaxHPLimit { get; private set; }


    [field: Header("부위 파괴 시 감소되는 최대 내구도 량")]
    [field: SerializeField] public float HeadMaxHPReduce_AD { get; private set; }
    [field: SerializeField] public float ArmMaxHPReduce_AD { get; private set; }
    [field: SerializeField] public float LegMaxHPReduce_AD { get; private set; }

    
}

[System.Serializable]
public class BatterySystem
{
    [field: Header("배터리 최대량 초기값 설정")]
    [field: SerializeField] public float InitMaxBattery { get; private set; }

    [field: Header("배터리 최대량 최소값 제한(기절을 반복해도 보장받는 최솟값)")]
    [field: SerializeField] public float MinBatteryLimit { get; private set; }

    [field: Header("기절 후 배터리 감소량")]
    [field: SerializeField] public float MaxBatteryReduceAfterFaint { get; private set; }

    [field: Header("상태 별 틱당 감소량")]
    [field: SerializeField] public float DrainPerTick_Idle { get; private set; }
    [field: SerializeField] public float DrainPerTick_Sprint { get; private set; }


    [field: Header("특정 조건 별 배터리 회복량")]
    [field: SerializeField] public float RecoverAmount_MonsterSlay { get; private set; }
    [field: SerializeField] public float RecoverAmount_BossHit { get; private set; }

    // 아이템 사용으로 인한 회복은 아이템 클래스에서 진행
    // 인터랙션으로 인한 회복은 인터랙터블 오브젝트 클래스에서 진행

    // 모든 기믹에서 배터리 소모량이 같으면 여기서 진행
    // [SerializeField] private int decreaseAmout_Gimic; 

    // 기믹별로 배터리 소모량을 각기 다르게 설정하고 싶다면 => 기믹 자체에서 배터리감소() 실행
}

[System.Serializable]
public class WillPowerSystem
{
    [field: Header("정신력 최대량 초기값 설정")]
    [field: SerializeField] public float MaxWillPower_Init { get; private set; }


    [field: Header("상태 별 틱당 감소량")]
    [field: SerializeField] public float DrainPerTick_Idle { get; private set; }
    [field: SerializeField] public float DrainPerTick_Night { get; private set; }
    [field: SerializeField] public float HeadDamagePerTick { get; private set; }

    [field: Header("머리에 내구도를 주기 시작하는 정신력 수치 기준")]
    [field: SerializeField] public float WillDangerZone { get; private set; }



    [field: Header("이벤트 별 감소량")]
    [field: SerializeField] public float DecreaseAmount_CatEvent { get; private set; }

    // DecreaseAmount_CustomMonster; 기획에서 추가될 시 추가
}


