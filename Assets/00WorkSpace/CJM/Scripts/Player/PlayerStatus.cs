using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public StateMachine<PlayerStateTypes> stateMachine = new StateMachine<PlayerStateTypes>();

    [Header("초기값 세팅")]////////////////////////
    [SerializeField] private int willPower_Init;
    [SerializeField] private int battery_Init;
    [SerializeField] private int damage_Init;
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float sprintSpeed_Init;
    [SerializeField] private float crouchSpeed_Init;
    [SerializeField] private float rotateSpeed_Init;
    [SerializeField] private float jumpForce_Init;
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity_Init;

    ///////////////////////////////////////////////

    [HideInInspector] public ObservableProperty<int> WillPower = new ObservableProperty<int>();
    [HideInInspector] public ObservableProperty<int> Battery = new ObservableProperty<int>();
    private Dictionary<BodyPartTypes, BodyPart> bodyParts;

    //private Inventory inventory;


    [field: SerializeField, Header("현재 플레이어 정보")] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float CrouchSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }

    [field: SerializeField] public int Damage { get; set; }


    [field: SerializeField] public int CurWillPower { get; private set; } 
    [field: SerializeField] public int CurBattery { get; private set; } 



    [Header("마우스 감도")]
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;

    [Header("마우스 수직 회전 각도 제한")]
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    
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
        CrouchSpeed = crouchSpeed_Init;
        RotateSpeed = rotateSpeed_Init;
        JumpForce = jumpForce_Init;

        // 인벤토리 기본 아이템만 있거나 전부 비우기.
    }

    public void WillPowerChanged(int value)
    {
        // 임시
        // CurWillPower필드 삭제 후 UI로 표기
        CurWillPower = WillPower.Value;
    }
    public void BatteryChanged(int value)
    {
        // 임시
        // CurBattery필드 삭제 후 UI로 표기
        CurBattery = Battery.Value;
    }

    

}
public enum PlayerStateTypes
{
    Idle, Attack, Damaged, Dead, Move, Sprint, Jump, Fall, Crouch //Exhausted,
}
public enum BodyPartTypes
{
    Head, Body, LeftArm, RightArm, LeftLeg, RightLeg
}