using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public StateMachine<PlayerStateTypes> stateMachine = new StateMachine<PlayerStateTypes>();

    SuvivalSystemManager ssm;

    [Header("런타임 내 불변 값 세팅")] /////////////////////
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    [SerializeField] private float rotateSpeed_Init;
    [SerializeField] private float crouchSpeed_Init;
    [field: SerializeField] public float DamagedInvincibleTime { get; private set; }
    [field: SerializeField] public float AttackCoolTime { get; private set; }
    public float CrouchSpeed { get { return crouchSpeed_Init; } }
    public float RotateSpeed { get { return rotateSpeed_Init; } }
    //////////////////////////////////////////////////////

    [Header("초기값 세팅")]////////////////////////
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float sprintSpeed_Init;
    [SerializeField] private float jumpForce_Init;
    [SerializeField] private int damage_Init;
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity_Init;
    ///////////////////////////////////////////////



    [Header("현재 플레이어 정보")] // 세이브 & 로드 가능
    public ObservableProperty<int> CurrentWillPower = new ObservableProperty<int>();
    public ObservableProperty<int> CurrentBattery = new ObservableProperty<int>();
    public ObservableProperty<int> MaxBattery = new ObservableProperty<int>();
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;

    [Header("장착 중인 아이템")]
    public Item onHandItem;

    // 우선 캔버스에 직접 연결하지만, MVP 구조로 리팩토링 필요 (데이터 & UI & 로직 처리(상태 업데이트, Input처리 등)로 분리)
    public InventoryPresenter inventory;


    [Header("신체 부위 데이터")]
    [SerializeField] private List<BodyPart> bodyParts;

    public bool ApplyDebuff_CraftSpeed;
    public bool ApplyDebuff_SprintSpeed;
    public bool ApplyDebuff_LockWeaponUse;
    public bool ApplyDebuff_LockSprint;
    public bool ApplyDebuff_LockAttack;
    public bool ApplyDebuff_HeadDotDamage;

    [Header("무적 상태 (피격 후 또는 컷씬 도중)")]
    public bool isInvincible;

    [Header("조작 가능/불가 상태")]
    public bool isControllLocked; // 고양이한테 물리거나 주인한테 잡힐 때 사용



    private void Update()
    {
        /// 테스트
        //Debug.Log(inventory.model.questSlots[2].item);
    }


    public float MinPitch { get { return minPitch; } }
    public float MaxPitch { get { return maxPitch; } }
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }


    // 플레이어 데이터 초기 상태
    public void Init()
    {
        ssm = SuvivalSystemManager.Instance;

        // 정신력 초기화
        CurrentWillPower.Value = ssm.willPowerSystem.MaxWillPower_Init;

        // 배터리 초기화
        InitBattery();


        MouseSensitivity = mouseSensitivity_Init;

        Damage = damage_Init;
        MoveSpeed = moveSpeed_Init;
        SprintSpeed = sprintSpeed_Init;
        JumpForce = jumpForce_Init;

        BodyPartsInit();

        // 인벤토리 초기화
        inventory = new InventoryPresenter();
    }

    // 플레이어 죽고 리스폰 할 때 초기화
    public void Init_AfterDead()
    {
        // 일반 로드 함수 실행
        Debug.Log("마지막에 저장한 데이터 로드해서 붙이기");

        // 정신력, 배터리만 최대로 맞춰주기
        CurrentWillPower.Value = ssm.willPowerSystem.MaxWillPower_Init;
        InitBattery();

        // 감소한 최대 체력으로 설정
        BodyPartsInit_AfterDead();
    }

    // 플레이어 기절 후 리스폰 할 때 초기화
    public void Init_AfterFaint()
    {
        // 최대 배터리 감소
        MaxBattery.Value = ssm.batterySystem.MaxBattery_AfterFaint;
        CurrentBattery.Value = MaxBattery.Value;
    }


    public void BodyPartsInit()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;
        List<BodyPart> tempBodyParts = new List<BodyPart>();

        // 신체 부위 별 최대 체력 따로 변수 만들어서 설정하자.
        tempBodyParts.Add(new BodyPart(BodyPartTypes.Head, ssm.bodyPartSystem.HeadMaxHP_Init, 
            (isActive) => Dead()));
        tempBodyParts.Add(new BodyPart(BodyPartTypes.LeftArm, ssm.bodyPartSystem.ArmMaxHP_Init, 
            (isActive) => ApplyDebuff_CraftSpeed = !isActive)); 
        tempBodyParts.Add(new BodyPart(BodyPartTypes.RightArm, ssm.bodyPartSystem.ArmMaxHP_Init,
            (isActive) => ApplyDebuff_LockWeaponUse = !isActive));
        tempBodyParts.Add(new BodyPart(BodyPartTypes.LeftLeg, ssm.bodyPartSystem.LegMaxHP_Init, 
            (isActive) => ApplyDebuff_SprintSpeed = !isActive));
        tempBodyParts.Add(new BodyPart(BodyPartTypes.RightLeg, ssm.bodyPartSystem.LegMaxHP_Init, 
            (isActive) => ApplyDebuff_SprintSpeed = !isActive));

        bodyParts = tempBodyParts;
    }

    public void BodyPartsInit_AfterDead()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;
        List<BodyPart> tempBodyParts = new List<BodyPart>();

        // 신체 부위 별 최대 체력 따로 변수 만들어서 설정하자.
        tempBodyParts.Add(new BodyPart(BodyPartTypes.Head, ssm.bodyPartSystem.HeadMaxHP_AfterDestroyed,
            (isActive) => Dead()));
        tempBodyParts.Add(new BodyPart(BodyPartTypes.LeftArm, ssm.bodyPartSystem.ArmMaxHP_AfterDestroyed,
            (isActive) => ApplyDebuff_CraftSpeed = !isActive));
        tempBodyParts.Add(new BodyPart(BodyPartTypes.RightArm, ssm.bodyPartSystem.ArmMaxHP_AfterDestroyed,
            (isActive) => ApplyDebuff_LockWeaponUse = !isActive));
        tempBodyParts.Add(new BodyPart(BodyPartTypes.LeftLeg, ssm.bodyPartSystem.LegMaxHP_AfterDestroyed,
            (isActive) => ApplyDebuff_SprintSpeed = !isActive));
        tempBodyParts.Add(new BodyPart(BodyPartTypes.RightLeg, ssm.bodyPartSystem.LegMaxHP_AfterDestroyed,
            (isActive) => ApplyDebuff_SprintSpeed = !isActive));

        bodyParts = tempBodyParts;

    }

    public void InitBattery()
    {
        MaxBattery.Value = ssm.batterySystem.MaxBattery_Init;
        CurrentBattery.Value = MaxBattery.Value;
    }

    public void ChargeBattery(int amount)
    {
        if (CurrentBattery.Value + amount < MaxBattery.Value)
            CurrentBattery.Value += amount;
        else CurrentBattery.Value = MaxBattery.Value;
    }

    public List<BodyPart> GetBodyPartsList()
    {
        return bodyParts;
    }

    // 몸 부위 중 2개 이상이 비활성화 상태일 때 디버프 효과 적용
    public void CheckCriticalState()
    {
        BodyPart leftArm = GetPart(BodyPartTypes.LeftArm);
        BodyPart rightArm = GetPart(BodyPartTypes.RightArm);
        BodyPart leftLeg = GetPart(BodyPartTypes.LeftLeg);
        BodyPart rightLeg = GetPart(BodyPartTypes.RightLeg);

        // 두 팔 모두 비활성화 상태일 때 => 공격 제한 디버프 활성화
        if (!leftArm.Activate.Value && !rightArm.Activate.Value) ApplyDebuff_LockAttack = true;
        else ApplyDebuff_LockAttack = false;

        // 두 다리 모두 비활성화 상태일 때 => 달리기 제한 디버프 활성화
        if (!leftLeg.Activate.Value && !rightLeg.Activate.Value) ApplyDebuff_LockSprint = true;
        else ApplyDebuff_LockSprint = false;

        // 머리 제외, 모든 부위가 비활성화 상태일 때 => 머리에 도트 데미지 디버프 활성화
        if (!leftLeg.Activate.Value && !rightLeg.Activate.Value && !leftArm.Activate.Value && !rightArm.Activate.Value)
            ApplyDebuff_HeadDotDamage = true;
    }

    public BodyPart GetPart(BodyPartTypes partType)
    {
        return bodyParts.Find(p => p.type == partType);
    }
    public void Dead()
    {
        PlayerManager.Instance.PlayerDead();
    }

    public bool IsCurrentState(PlayerStateTypes state)
    {
        return stateMachine.CurState == stateMachine.stateDic[state];
    }
}
public enum PlayerStateTypes
{
    Idle, Attack, Damaged, Dead, Move, Sprint, Jump, Fall, Crouch //Exhausted,
}
public enum BodyPartTypes
{
    Head, LeftArm, RightArm, LeftLeg, RightLeg
}