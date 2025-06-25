using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public StateMachine<PlayerStateTypes> stateMachine = new StateMachine<PlayerStateTypes>();

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
    [SerializeField] private int willPower_Init;
    [SerializeField] private int battery_Init;
    [SerializeField] private float moveSpeed_Init;
    [SerializeField] private float sprintSpeed_Init;
    [SerializeField] private float jumpForce_Init;
    [SerializeField] private int damage_Init;
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity_Init;
    ///////////////////////////////////////////////



    [Header("���� �÷��̾� ����")] // ���̺� & �ε� ����
    public ObservableProperty<int> WillPower = new ObservableProperty<int>();
    public ObservableProperty<int> Battery = new ObservableProperty<int>();
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;
    

    // �켱 ĵ������ ���� ����������, MVP ������ �����丵 �ʿ� (������ & UI & ���� ó��(���� ������Ʈ, Inputó�� ��)�� �и�)
    public Inventory inventory;


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
    }


    public float MinPitch { get { return minPitch; } }
    public float MaxPitch { get { return maxPitch; } }
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }


    // �÷��̾� ������ �ʱ� ����
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

        // �κ��丮 �⺻ �����۸� �ְų� ���� ����.

        BodySet();
    }

    public void WillPowerChanged(int value)
    {
        // �ӽ�
        // CurWillPower�ʵ� ���� �� UI�� ǥ��
    }
    public void BatteryChanged(int value)
    {
        // �ӽ�
        // CurBattery�ʵ� ���� �� UI�� ǥ��
    }

    public void BodySet()
    {
        // ��ü ���� �� �ִ� ü�� ���� ���� ���� ��������.
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
        Debug.Log("�÷��̾� ���");
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