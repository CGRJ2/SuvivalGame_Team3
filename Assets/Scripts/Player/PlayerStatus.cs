using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[System.Serializable] // ���̺� & �ε� ����
public class PlayerStatus 
{

    SuvivalSystemManager ssm;

    [Header("���� ���� ������")]
    public Item onHandItem;

    [Header("�÷��̾� ���� ��ġ ����")]
    public ObservableProperty<int> CurrentWillPower = new ObservableProperty<int>();
    public ObservableProperty<int> CurrentBattery = new ObservableProperty<int>();
    public ObservableProperty<int> MaxBattery = new ObservableProperty<int>();
    [Header("��ü ���� ������")]
    [SerializeField] private List<BodyPart> bodyParts;


    [field: Header("�÷��̾� ���� ����")] 
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public int Damage { get; set; }


    // �켱 ĵ������ ���� ����������, MVP ������ �����丵 �ʿ� (������ & UI & ���� ó��(���� ������Ʈ, Inputó�� ��)�� �и�)
    public InventoryPresenter inventory;

    [Header("����� ����")]
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


    [Header("������ ���콺 ����(����x)")]
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }


    // �÷��̾� ������ �ʱ� ����
    public void Init()
    {
        ssm = SuvivalSystemManager.Instance;
        PlayerManager pm = PlayerManager.Instance;

        // ���ŷ� �ʱ�ȭ
        CurrentWillPower.Value = ssm.willPowerSystem.MaxWillPower_Init;

        // ���͸� �ʱ�ȭ
        InitBattery();


        MouseSensitivity = pm.mouseSensitivity_Init;

        Damage = pm.damage_Init;
        MoveSpeed = pm.moveSpeed_Init;
        SprintSpeed = pm.sprintSpeed_Init;
        JumpForce = pm.jumpForce_Init;

        BodyPartsInit();

        // �κ��丮 �ʱ�ȭ
        inventory = new InventoryPresenter();
    }




    // �÷��̾� �װ� ������ �� �� �ʱ�ȭ
    public void Init_AfterDead()
    {
        // �Ϲ� �ε� �Լ� ����
        Debug.Log("�������� ������ ������ �ε��ؼ� ���̱�");

        // ���ŷ�, ���͸��� �ִ�� �����ֱ�
        CurrentWillPower.Value = ssm.willPowerSystem.MaxWillPower_Init;
        InitBattery();

        // ������ �ִ� ü������ ����
        BodyPartsInit_AfterDead();
    }

    // �÷��̾� ���� �� ������ �� �� �ʱ�ȭ
    public void Init_AfterFaint()
    {
        // �ִ� ���͸� ����
        MaxBattery.Value = ssm.batterySystem.MaxBattery_AfterFaint;
        CurrentBattery.Value = MaxBattery.Value;
    }


    public void BodyPartsInit()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;
        List<BodyPart> tempBodyParts = new List<BodyPart>();

        // ��ü ���� �� �ִ� ü�� ���� ���� ���� ��������.

        // �Ӹ�
        BodyPart head = new BodyPart(BodyPartTypes.Head, ssm.bodyPartSystem.HeadMaxHP_Init);
        head.Activate.Subscribe(Dead);
        tempBodyParts.Add(head);

        // ����
        BodyPart leftArm = new BodyPart(BodyPartTypes.LeftArm, ssm.bodyPartSystem.ArmMaxHP_Init);
        leftArm.Activate.Subscribe(Debuff_CraftSpeed);
        tempBodyParts.Add(leftArm);

        // ������
        BodyPart rightArm = new BodyPart(BodyPartTypes.RightArm, ssm.bodyPartSystem.ArmMaxHP_Init);
        rightArm.Activate.Subscribe(Debuff_LockWeaponUse);
        tempBodyParts.Add(rightArm);

        // �޴ٸ�
        BodyPart leftLeg = new BodyPart(BodyPartTypes.LeftLeg, ssm.bodyPartSystem.LegMaxHP_Init);
        leftLeg.Activate.Subscribe(Debuff_SprintSpeed);
        tempBodyParts.Add(leftLeg);

        // �����ٸ�
        BodyPart rightLeg = new BodyPart(BodyPartTypes.RightLeg, ssm.bodyPartSystem.LegMaxHP_Init);
        rightLeg.Activate.Subscribe(Debuff_SprintSpeed);
        tempBodyParts.Add(rightLeg);

        bodyParts = tempBodyParts;
    }

    // ����� ȿ����
    public void Debuff_CraftSpeed(bool isActive)
    {
        // ������ ��Ȱ��ȭ �Ǹ�
        if (!isActive)
        {
            // ����� ����
            ApplyDebuff_CraftSpeed = true;
        }
        else
            ApplyDebuff_CraftSpeed = false;


    }
    public void Debuff_LockWeaponUse(bool isActive)
    {
        // ������ ��Ȱ��ȭ �Ǹ�
        if (!isActive)
        {
            // ����� ����
            ApplyDebuff_LockWeaponUse = true;
        }
        else
            ApplyDebuff_LockWeaponUse = false;
    }
    public void Debuff_SprintSpeed(bool isActive)
    {
        // ������ ��Ȱ��ȭ �Ǹ�
        if (!isActive)
        {
            // ����� ����
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