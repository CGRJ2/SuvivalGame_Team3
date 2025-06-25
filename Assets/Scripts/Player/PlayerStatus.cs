using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public StateMachine<PlayerStateTypes> stateMachine = new StateMachine<PlayerStateTypes>();

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
    [SerializeField] private int willPower_Init;
    [SerializeField] private int battery_Init;
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float sprintSpeed_Init;
    [SerializeField] private float jumpForce_Init;
    [SerializeField] private int damage_Init;
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity_Init;
    ///////////////////////////////////////////////



    [Header("현재 플레이어 정보")] // 세이브 & 로드 가능
    public ObservableProperty<int> WillPower = new ObservableProperty<int>();
    public ObservableProperty<int> Battery = new ObservableProperty<int>();
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;
    

    // 우선 캔버스에 직접 연결하지만, MVP 구조로 리팩토링 필요 (데이터 & UI & 로직 처리(상태 업데이트, Input처리 등)로 분리)
    public Inventory inventory;


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
    }


    public float MinPitch { get { return minPitch; } }
    public float MaxPitch { get { return maxPitch; } }
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }


    // 플레이어 데이터 초기 상태
    public void Init()
    {
        WillPower.Subscribe(WillPowerChanged);
        Battery.Subscribe(BatteryChanged);

        WillPower.Value = willPower_Init;
        Battery.Value = battery_Init;

        MouseSensitivity = mouseSensitivity_Init;

        Damage = damage_Init;
        MoveSpeed = moveSpeed_Init;
        SprintSpeed = sprintSpeed_Init;
        JumpForce = jumpForce_Init;

        // 인벤토리 기본 아이템만 있거나 전부 비우기.

        BodySet();
    }

    public void WillPowerChanged(int value)
    {
        // 임시
        // CurWillPower필드 삭제 후 UI로 표기
    }
    public void BatteryChanged(int value)
    {
        // 임시
        // CurBattery필드 삭제 후 UI로 표기
    }

    public void BodySet()
    {
        // 신체 부위 별 최대 체력 따로 변수 만들어서 설정하자.
        bodyParts.Add(new BodyPart(BodyPartTypes.LeftArm, 10, (isActive) => ApplyDebuff_CraftSpeed = !isActive)); 
        bodyParts.Add(new BodyPart(BodyPartTypes.RightArm, 10, (isActive) => ApplyDebuff_LockWeaponUse = !isActive));
        bodyParts.Add(new BodyPart(BodyPartTypes.LeftLeg, 10, (isActive) => ApplyDebuff_SprintSpeed = !isActive));
        bodyParts.Add(new BodyPart(BodyPartTypes.RightLeg, 10, (isActive) => ApplyDebuff_SprintSpeed = !isActive));
        bodyParts.Add(new BodyPart(BodyPartTypes.Head, 100, (isActive) => Dead()));
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
        Debug.Log("플레이어 사망");
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