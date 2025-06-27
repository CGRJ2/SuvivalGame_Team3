using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public StateMachine<PlayerStateTypes> stateMachine = new StateMachine<PlayerStateTypes>();

    SuvivalSystemManager ssm;

    [Header("��Ÿ�� �� �Һ� �� ����")] /////////////////////
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    [SerializeField] private float rotateSpeed_Init;
    [SerializeField] private float crouchSpeed_Init;
    [field: SerializeField] public float DamagedInvincibleTime { get; private set; }
    [field: SerializeField] public float AttackCoolTime { get; private set; }
    public float CrouchSpeed { get { return crouchSpeed_Init; } }
    public float RotateSpeed { get { return rotateSpeed_Init; } }
    //////////////////////////////////////////////////////

    [Header("�ʱⰪ ����")]////////////////////////
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float sprintSpeed_Init;
    [SerializeField] private float jumpForce_Init;
    [SerializeField] private int damage_Init;
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity_Init;
    ///////////////////////////////////////////////



    [Header("���� �÷��̾� ����")] // ���̺� & �ε� ����
    public ObservableProperty<int> CurrentWillPower = new ObservableProperty<int>();
    public ObservableProperty<int> CurrentBattery = new ObservableProperty<int>();
    public ObservableProperty<int> MaxBattery = new ObservableProperty<int>();
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;
    

    // �켱 ĵ������ ���� ����������, MVP ������ �����丵 �ʿ� (������ & UI & ���� ó��(���� ������Ʈ, Inputó�� ��)�� �и�)
    public InventoryPresenter inventory;


    [Header("��ü ���� ������")]
    [SerializeField] private List<BodyPart> bodyParts;

    public bool ApplyDebuff_CraftSpeed;
    public bool ApplyDebuff_SprintSpeed;
    public bool ApplyDebuff_LockWeaponUse;
    public bool ApplyDebuff_LockSprint;
    public bool ApplyDebuff_LockAttack;
    public bool ApplyDebuff_HeadDotDamage;

    [Header("���� ���� (�ǰ� �� �Ǵ� �ƾ� ����)")]
    public bool isInvincible;

    [Header("���� ����/�Ұ� ����")]
    public bool isControllLocked; // ��������� �����ų� �������� ���� �� ���



    private void Update()
    {
        /// �׽�Ʈ
        //Debug.Log(inventory.model.questSlots[2].item);
    }


    public float MinPitch { get { return minPitch; } }
    public float MaxPitch { get { return maxPitch; } }
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }


    // �÷��̾� ������ �ʱ� ����
    public void Init()
    {
        ssm = SuvivalSystemManager.Instance;

        // ���ŷ� �ʱ�ȭ
        CurrentWillPower.Value = ssm.willPowerSystem.MaxWillPower_Init;

        // ���͸� �ʱ�ȭ
        InitBattery();


        MouseSensitivity = mouseSensitivity_Init;

        Damage = damage_Init;
        MoveSpeed = moveSpeed_Init;
        SprintSpeed = sprintSpeed_Init;
        JumpForce = jumpForce_Init;

        BodyPartsInit();

        // �κ��丮 �ʱ�ȭ
        inventory = new InventoryPresenter();
    }

    public void BodyPartsInit()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;

        // ��ü ���� �� �ִ� ü�� ���� ���� ���� ��������.
        bodyParts.Add(new BodyPart(BodyPartTypes.Head, ssm.bodyPartSystem.HeadMaxHP_Init, 
            (isActive) => Dead()));
        bodyParts.Add(new BodyPart(BodyPartTypes.LeftArm, ssm.bodyPartSystem.ArmMaxHP_Init, 
            (isActive) => ApplyDebuff_CraftSpeed = !isActive)); 
        bodyParts.Add(new BodyPart(BodyPartTypes.RightArm, ssm.bodyPartSystem.ArmMaxHP_Init,
            (isActive) => ApplyDebuff_LockWeaponUse = !isActive));
        bodyParts.Add(new BodyPart(BodyPartTypes.LeftLeg, ssm.bodyPartSystem.LegMaxHP_Init, 
            (isActive) => ApplyDebuff_SprintSpeed = !isActive));
        bodyParts.Add(new BodyPart(BodyPartTypes.RightLeg, ssm.bodyPartSystem.LegMaxHP_Init, 
            (isActive) => ApplyDebuff_SprintSpeed = !isActive));
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

    // �� ���� �� 2�� �̻��� ��Ȱ��ȭ ������ �� ����� ȿ�� ����
    public void CheckCriticalState()
    {
        BodyPart leftArm = GetPart(BodyPartTypes.LeftArm);
        BodyPart rightArm = GetPart(BodyPartTypes.RightArm);
        BodyPart leftLeg = GetPart(BodyPartTypes.LeftLeg);
        BodyPart rightLeg = GetPart(BodyPartTypes.RightLeg);

        // �� �� ��� ��Ȱ��ȭ ������ �� => ���� ���� ����� Ȱ��ȭ
        if (!leftArm.Activate.Value && !rightArm.Activate.Value) ApplyDebuff_LockAttack = true;
        else ApplyDebuff_LockAttack = false;

        // �� �ٸ� ��� ��Ȱ��ȭ ������ �� => �޸��� ���� ����� Ȱ��ȭ
        if (!leftLeg.Activate.Value && !rightLeg.Activate.Value) ApplyDebuff_LockSprint = true;
        else ApplyDebuff_LockSprint = false;

        // �Ӹ� ����, ��� ������ ��Ȱ��ȭ ������ �� => �Ӹ��� ��Ʈ ������ ����� Ȱ��ȭ
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