using System.Collections;
using UnityEngine;

public class SuvivalSystemManager : Singleton<SuvivalSystemManager>
{
    // 참조한 다른 매니저들보다 후 순위에서 초기화 해야 함
    PlayerManager pm;
    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }
    ///////////////////////////////////////////

    [Header("생존 수치 소모 주기 설정")]
    [SerializeField] private float TickDuration;

    [System.Serializable]
    public class BodyPartSystem
    {
        [field: Header("기본 최대 체력 초기값 설정")]
        [field: SerializeField] public int HeadMaxHP_Init { get; private set; }
        [field: SerializeField] public int ArmMaxHP_Init { get; private set; }
        [field: SerializeField] public int LegMaxHP_Init { get; private set; }


        [field: Header("파괴 상태에서 1차 회복 사용 시 최대체력 제한 값 설정")]
        [field: SerializeField] public int HeadMaxHP_AfterDestroyed { get; private set; }
        [field: SerializeField] public int ArmMaxHP_AfterDestroyed { get; private set; }
        [field: SerializeField] public int LegMaxHP_AfterDestroyed { get; private set; }
    }

    [System.Serializable]
    public class BatterySystem
    {
        [field: Header("정신력 최대량 초기값 설정")]
        [field: SerializeField] public int MaxBattery_Init { get; private set; }

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
    public class WillPowerSystem
    {
        [field: Header("정신력 최대량 초기값 설정")]
        [field: SerializeField] public int MaxWillPower_Init { get; private set; }


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

    [field: Header("부위별 내구도 시스템")]
    [field: SerializeField] public BodyPartSystem bodyPartSystem { get; private set; }

    [field: Header("배터리 시스템")]
    [field: SerializeField] public BatterySystem batterySystem { get; private set; }

    [field: Header("정신력 시스템")]
    [field: SerializeField] public WillPowerSystem willPowerSystem { get; private set; }

    Coroutine coroutine_DecreaseWillPower;
    Coroutine coroutine_DecreaseBattery;
    Coroutine coroutine_DotDamageByLowWill;

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

    // 배터리 지속 감소 루틴
    IEnumerator DecreaseBatteryOverTime()
    {
        PlayerStatus ps = pm.instancePlayer.Status;

        while (true)
        {
            yield return new WaitForSeconds(TickDuration);

            // 달리기 상태라면
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


    // 정신력이 일정수치 이하일 때 머리 지속 데미지 루틱
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


    // 배터리 교체 (초기화)
    public void InitBattery()
    {
        pm.instancePlayer.Status.InitBattery();
    }

    // 배터리 회복
    public void RecoveryBattery(int amount)
    {
        pm.instancePlayer.Status.ChargeBattery(amount);
    }


    // 파츠 교체 (초기화)
    public void InitBodyPart(BodyPart bodyPart)
    {
        bodyPart.Init();
    }

    // 파츠 회복
    public void RepairBodyPart(BodyPart bodyPart, int amount)
    {
        bodyPart.Repair(amount);
    }

    // 파츠 활성화 (간이 회복)
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
