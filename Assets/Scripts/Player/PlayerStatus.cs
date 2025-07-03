using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[System.Serializable] // 세이브 & 로드 가능
public class PlayerStatus 
{

    SuvivalSystemManager ssm;

    [Header("장착 중인 아이템")]
    public Item onHandItem;

    [Header("플레이어 생존 수치 정보")]
    public ObservableProperty<int> CurrentWillPower = new ObservableProperty<int>();
    public ObservableProperty<int> CurrentBattery = new ObservableProperty<int>();
    public ObservableProperty<int> MaxBattery = new ObservableProperty<int>();
    [Header("신체 부위 데이터")]
    [SerializeField] private List<BodyPart> bodyParts;


    [field: Header("플레이어 스탯 정보")] 
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public int Damage { get; set; }


    // 우선 캔버스에 직접 연결하지만, MVP 구조로 리팩토링 필요 (데이터 & UI & 로직 처리(상태 업데이트, Input처리 등)로 분리)
    public InventoryPresenter inventory;

    [Header("디버프 상태")]
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


    [Header("설정된 마우스 감도(구현x)")]
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }


    // 플레이어 데이터 초기 상태
    public void Init()
    {
        ssm = SuvivalSystemManager.Instance;
        PlayerManager pm = PlayerManager.Instance;

        // 정신력 초기화
        CurrentWillPower.Value = ssm.willPowerSystem.MaxWillPower_Init;

        // 배터리 초기화
        InitBattery();


        MouseSensitivity = pm.mouseSensitivity_Init;

        Damage = pm.damage_Init;
        MoveSpeed = pm.moveSpeed_Init;
        SprintSpeed = pm.sprintSpeed_Init;
        JumpForce = pm.jumpForce_Init;

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

        // 머리
        BodyPart head = new BodyPart(BodyPartTypes.Head, ssm.bodyPartSystem.HeadMaxHP_Init);
        head.Activate.Subscribe(Dead);
        tempBodyParts.Add(head);

        // 왼팔
        BodyPart leftArm = new BodyPart(BodyPartTypes.LeftArm, ssm.bodyPartSystem.ArmMaxHP_Init);
        leftArm.Activate.Subscribe(Debuff_CraftSpeed);
        tempBodyParts.Add(leftArm);

        // 오른팔
        BodyPart rightArm = new BodyPart(BodyPartTypes.RightArm, ssm.bodyPartSystem.ArmMaxHP_Init);
        rightArm.Activate.Subscribe(Debuff_LockWeaponUse);
        tempBodyParts.Add(rightArm);

        // 왼다리
        BodyPart leftLeg = new BodyPart(BodyPartTypes.LeftLeg, ssm.bodyPartSystem.LegMaxHP_Init);
        leftLeg.Activate.Subscribe(Debuff_SprintSpeed);
        tempBodyParts.Add(leftLeg);

        // 오른다리
        BodyPart rightLeg = new BodyPart(BodyPartTypes.RightLeg, ssm.bodyPartSystem.LegMaxHP_Init);
        rightLeg.Activate.Subscribe(Debuff_SprintSpeed);
        tempBodyParts.Add(rightLeg);

        bodyParts = tempBodyParts;
    }

    // 디버프 효과들
    public void Debuff_CraftSpeed(bool isActive)
    {
        // 파츠가 비활성화 되면
        if (!isActive)
        {
            // 디버프 적용
            ApplyDebuff_CraftSpeed = true;
        }
        else
            ApplyDebuff_CraftSpeed = false;


    }
    public void Debuff_LockWeaponUse(bool isActive)
    {
        // 파츠가 비활성화 되면
        if (!isActive)
        {
            // 디버프 적용
            ApplyDebuff_LockWeaponUse = true;
        }
        else
            ApplyDebuff_LockWeaponUse = false;
    }
    public void Debuff_SprintSpeed(bool isActive)
    {
        // 파츠가 비활성화 되면
        if (!isActive)
        {
            // 디버프 적용
            ApplyDebuff_SprintSpeed = true;
        }
        else
            ApplyDebuff_SprintSpeed = false;
    }
    //


    public void BodyPartsInit_AfterDead()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;
        for(int i = 0; i < bodyParts.Count; i++)
        {
            switch (bodyParts[i].type)
            {
                case BodyPartTypes.Head:
                    bodyParts[i].CurrentMaxHp = ssm.bodyPartSystem.HeadMaxHP_AfterDestroyed;
                    break;

                case BodyPartTypes.LeftArm:
                case BodyPartTypes.RightArm:
                    bodyParts[i].CurrentMaxHp = ssm.bodyPartSystem.ArmMaxHP_AfterDestroyed;
                    break;

                case BodyPartTypes.LeftLeg:
                case BodyPartTypes.RightLeg:
                    bodyParts[i].CurrentMaxHp = ssm.bodyPartSystem.LegMaxHP_AfterDestroyed;
                    break;
            }
        } 

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

    public void Dead(bool isActive)
    {
        if (!isActive)
            PlayerManager.Instance.PlayerDead();
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