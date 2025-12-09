using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 세이브 & 로드 가능
public class PlayerModel : IDisposable
{
    [Header("장착 중인 아이템")]
    public Item onHandItem;

    [Header("플레이어 생존 수치 정보")]
    public ObservableProperty<float> CurrentWillPower = new ObservableProperty<float>();
    public ObservableProperty<float> CurrentBattery = new ObservableProperty<float>();
    public ObservableProperty<float> MaxBattery = new ObservableProperty<float>();
    public ObservableProperty<float> SumCurrentHP = new ObservableProperty<float>();
    public ObservableProperty<float> SumCurrentMaxHP = new ObservableProperty<float>();

    
    private Dictionary<BodyPartType, BodyPart> _bodyPartsDic = new();

    [field: Header("플레이어 스탯 런타임 데이터")]
    [field: SerializeField] public float WalkSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }
    [field: SerializeField] public float CrouchSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public float Damage { get; set; }

    // 인벤토리 데이터
    [HideInInspector] public InventoryPresenter inventory;

    [Header("무적 상태 (피격 후 또는 컷씬 도중)")]
    public bool isInvincible;

    [Header("조작 가능/불가 상태")]
    public bool isControllLocked;



    // 상태와 함께 쓰일 플래그
    public bool IsCrouching { get; private set; }
    public bool IsSprinting { get; private set; }


    [Header("설정된 마우스 감도(구현x)")]
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }


    public bool IsDead;
    public event Action OnDied;
    
    public bool IsFaint;
    public event Action OnFaint;

    public bool IsGrounded; // 이 놈들은 Status로 분리하는게 좋을 듯
    public bool IsRolling;
    public bool IsLanding;


    public bool IsInvincible;


    // 플레이어 데이터 초기 상태
    public void Init()
    {
        // 정신력 초기화
        CurrentWillPower.Value = Manager.data.CapacityTable.FindByKey("Will").Max;

        // 배터리 초기화
        InitBattery();

        // 스탯 초기화
        var initStatTable = Manager.data.PlayerStatsTable;
        WalkSpeed   = initStatTable.FindByKey("MoveSpeed").InitValue;
        SprintSpeed = initStatTable.FindByKey("SprintSpeed").InitValue;
        RotateSpeed = initStatTable.FindByKey("RotateSpeed").InitValue;
        CrouchSpeed = initStatTable.FindByKey("CrouchSpeed").InitValue;
        JumpForce   = initStatTable.FindByKey("JumpForce").InitValue;
        Damage      = initStatTable.FindByKey("Damage").InitValue;

        // 신체 부위 초기화
        BodyPartsInit();

        // 인벤토리 초기화
        inventory = new InventoryPresenter();
    }

    // 데이터 로드 시에만 초기화 할 것들
    public void Init_Load()
    {
        BodyPartsInit();
    }


    // 플레이어 죽고 리스폰 할 때 초기화
    public void Respawn_Dead()
    {
        // 정신력, 배터리만 최대로 맞춰주기
        CurrentWillPower.Value = Manager.data.CapacityTable.FindByKey("Will").Max;
        InitBattery();

        // 감소한 최대 체력으로 설정
        BodyPartsInitInRespawn();
    }

    // 플레이어 기절 후 리스폰 할 때 초기화
    public void Respawn_Faint()
    {
        var batteryFixedData = Manager.data.CapacityTable.FindByKey("Battery");
        var reduceAmount = batteryFixedData.ReducePerTick;
        var min = batteryFixedData.Min;

        // 최대 배터리 감소
        if (MaxBattery.Value - reduceAmount > min)
            MaxBattery.Value -= reduceAmount;
        else
            MaxBattery.Value = min;

        CurrentBattery.Value = MaxBattery.Value;
    }
    
    // 신체 부위별 인스턴스 생성
    public void BodyPartsInit()
    {
        Panel_PlayerStatus playerStatusUI = Manager.ui.inventoryGroup.panel_PlayerStatus;

        var bodyStatsTable = Manager.data.BodyStatsTable;
        string bodyKey = "";

        for (int i = 0; i < Enum.GetValues(typeof(BodyPartType)).Length; i++)
        {
            switch ((BodyPartType)i)
            {
                case BodyPartType.Head:
                    bodyKey = "HeadHP";
                    break;

                case BodyPartType.LeftArm:
                case BodyPartType.RightArm:
                    bodyKey = "ArmHP";
                    break;

                case BodyPartType.LeftLeg:
                case BodyPartType.RightLeg:
                    bodyKey = "LegHP";
                    break;

                default: bodyKey = "";
                    break;
            }

            if (string.IsNullOrWhiteSpace(bodyKey))
            {
                Debug.LogError($"테이블에 존재하지 않는 신체부위 타입 존재: {(BodyPartType)i}");
                break;
            }

            var bodyPartData = bodyStatsTable.FindByKey(bodyKey);
            var reduceAmount = bodyPartData.ReduceAmount;
            var maxHP = bodyPartData.Max;
            var minHp = bodyPartData.Min;

            BodyPart bodyPart = new BodyPart((BodyPartType)i, maxHP);
            _bodyPartsDic[(BodyPartType)i] = bodyPart;
        }
    }

    // 사망 후 신체부위 최대 내구도 감소
    public void BodyPartsInitInRespawn()
    {
        var bodyStatsTable = Manager.data.BodyStatsTable;
        string bodyKey = "";

        foreach (var kvp in _bodyPartsDic)
        {
            switch (kvp.Key)
            {
                case BodyPartType.Head:
                    bodyKey = "HeadHP";
                    break;

                case BodyPartType.LeftArm:
                case BodyPartType.RightArm:
                    bodyKey = "ArmHP";
                    break;

                case BodyPartType.LeftLeg:
                case BodyPartType.RightLeg:
                    bodyKey = "LegHP";
                    break;
            }

            var bodyPartData = bodyStatsTable.FindByKey(bodyKey);
            var reduceAmount = bodyPartData.ReduceAmount;
            var minHp = bodyPartData.Min;

            if (_bodyPartsDic[kvp.Key].CurrentMaxHp.Value - reduceAmount > minHp)
                _bodyPartsDic[kvp.Key].CurrentMaxHp.Value -= reduceAmount;
            else
                _bodyPartsDic[kvp.Key].CurrentMaxHp.Value = minHp;

            _bodyPartsDic[kvp.Key].Hp = _bodyPartsDic[kvp.Key].CurrentMaxHp;
        }
    }

    public void InitBattery()
    {
        MaxBattery.Value = Manager.data.CapacityTable.FindByKey("Battery").Max;
        CurrentBattery.Value = MaxBattery.Value;
    }

    public void ChargeBattery(float amount)
    {
        if (CurrentBattery.Value + amount < MaxBattery.Value)
            CurrentBattery.Value += amount;
        else CurrentBattery.Value = MaxBattery.Value;
    }

    public Dictionary<BodyPartType, BodyPart> GetBodyPartsDic()
    {
        return _bodyPartsDic;
    }

    // 모든 부위의 현재 체력의 합을 Model 필드의 SumCurrentHP로 환산해주는 함수
    public void CalculateCurrentHPSum(float hp)
    {
        float sumHP = 0;
        foreach (var kvp in _bodyPartsDic)
        {
            sumHP += kvp.Value.Hp.Value;
        }
        SumCurrentHP.Value = sumHP;
    }

    // 모든 부위의 최대 체력의 합을 Model 필드의 SumCurrentMaxHP로 환산해주는 함수
    public void CalculateCurrentMaxHPSum(float hp)
    {
        float sumMaxHP = 0;
        foreach (var kvp in _bodyPartsDic)
        {
            sumMaxHP += kvp.Value.CurrentMaxHp.Value;
        }
        SumCurrentMaxHP.Value = sumMaxHP;
    }

    public void Tick(float tickDuration)
    {
        if (IsDead || IsFaint) return;
        if (CurrentBattery.Value > 0)
        {
            var idleBatteryConsume = Manager.data.BatteryConsumeTable.FindByKey("Idle");
            if (CurrentBattery.Value - idleBatteryConsume.Amount > 0f)
            {
                CurrentBattery.Value -= idleBatteryConsume.Amount;
            }
            else
            {
                CurrentBattery.Value = 0f;
                Faint();
            }
        }
    }

    //  부위 판정 시스템
    public void TakeDamage(float damage)
    {
        // 죽음 상태라면 데미지 계산 실행X
        if (IsDead) return;

        // 활성 상태인 신체 부위 중 랜덤 선택 -> 이것도 피격 부위 데미지 주는걸로 구현해볼까...
        List<BodyPart> activeBodyPart = new List<BodyPart>();

        // 활성 상태인 파츠들로 리스트 새로 생성 (이미 파괴된 부위를 제외하기 위함)
        foreach (var kvp in _bodyPartsDic)
        {
            if (kvp.Value.Activate.Value)
                activeBodyPart.Add(kvp.Value);
        }

        // 활성 상태 부위 랜덤 데미지
        if (activeBodyPart.Count > 0)
        {
            int r = UnityEngine.Random.Range(1, activeBodyPart.Count);
            activeBodyPart[r].TakeDamage(damage);
        }

        // 전체 체력 or 머리 체력이 0 이하면  사망
        if (SumCurrentHP.Value <= 0 || _bodyPartsDic[BodyPartType.Head].Hp.Value <= 0)
            Die();
    }

    public void Die()
    {
        if (IsDead) return;
        IsDead = true;
        OnDied?.Invoke();
    }

    // => 배터리가 0이 되었을 때 호출
    public void Faint()
    {
        if (IsFaint) return;
        IsFaint = true;
        OnFaint?.Invoke();
    }

    public void SetCrouch(bool crouch)
    {
        IsCrouching = crouch;

        // 규칙: 웅크렸으면 스프린트는 해제
        if (crouch)
            IsSprinting = false;
    }

    public void SetSprint(bool sprint)
    {
        // 규칙: 웅크린 상태에서는 스프린트 금지
        if (IsCrouching && sprint)
            return;

        IsSprinting = sprint;
    }

    public float GetCurrentMoveSpeed()
    {
        if (IsCrouching) return CrouchSpeed;
        if (IsSprinting) return SprintSpeed;
        return WalkSpeed;
    }

    public void ResetFlagsOnAirborne()
    {
        IsCrouching = false;
        IsSprinting = false;
    }

    public void Dispose()
    {
        CurrentWillPower.UnsubscribeAll();
        CurrentBattery.UnsubscribeAll();
        MaxBattery.UnsubscribeAll();
        SumCurrentHP.UnsubscribeAll();
        SumCurrentMaxHP.UnsubscribeAll();
    }
}

public enum BodyPartType
{
    Head, LeftArm, RightArm, LeftLeg, RightLeg
}