using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // ���̺� & �ε� ����
public class PlayerStatus : IDisposable
{
    [Header("���� ���� ������")]
    public Item onHandItem;

    [Header("�÷��̾� ���� ��ġ ����")]
    public ObservableProperty<float> CurrentWillPower = new ObservableProperty<float>();
    public ObservableProperty<float> CurrentBattery = new ObservableProperty<float>();
    public ObservableProperty<float> MaxBattery = new ObservableProperty<float>();
    public ObservableProperty<float> SumCurrentHP = new ObservableProperty<float>();
    public ObservableProperty<float> SumCurrentMaxHP = new ObservableProperty<float>();

    public void CalculateCurrentHPSum(float hp)
    {
        float sumHP = 0;
        foreach (BodyPart bodyPart in bodyParts)
        {
            sumHP += bodyPart.Hp.Value;
        }
        SumCurrentHP.Value = sumHP;
    }
    public void CalculateCurrentMaxHPSum(float hp)
    {
        float sumMaxHP = 0;
        foreach (BodyPart bodyPart in bodyParts)
        {
            sumMaxHP += bodyPart.CurrentMaxHp.Value;
        }
        SumCurrentMaxHP.Value = sumMaxHP;
    }


    [Header("��ü ���� ������")]
    [SerializeField] private List<BodyPart> bodyParts;


    [field: Header("�÷��̾� ���� ����")]
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public float Damage { get; set; }


    [SerializeField] public InventoryPresenter inventory;

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
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;
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

        Init_Load();

        // �κ��丮 �ʱ�ȭ
        inventory = new InventoryPresenter();
    }

    // ������ �ε� �ÿ��� �ʱ�ȭ �� �͵�
    public void Init_Load()
    {
        BodyPartsInit();

        CurrentBattery.Subscribe(PlayerManager.Instance.PlayerFaint);
    }


    // �÷��̾� �װ� ������ �� �� �ʱ�ȭ
    public void Init_AfterDead()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;

        // ���ŷ�, ���͸��� �ִ�� �����ֱ�
        CurrentWillPower.Value = ssm.willPowerSystem.MaxWillPower_Init;
        InitBattery();

        // ������ �ִ� ü������ ����
        BodyPartsInit_AfterDead();
    }

    // �÷��̾� ���� �� ������ �� �� �ʱ�ȭ
    public void Init_AfterFaint()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;

        Debug.Log("���� �� �ȵ�");

        // �ִ� ���͸� ����
        Debug.Log($"[MaxBattery.Value]1 : {MaxBattery.Value}");
        MaxBattery.Value = ssm.batterySystem.MaxBattery_AfterFaint;
        Debug.Log($"[MaxBattery.Value]2 : {MaxBattery.Value}");
        CurrentBattery.Value = MaxBattery.Value;
    }


    public void BodyPartsInit()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;
        Panel_PlayerStatus playerStatusUI = UIManager.Instance.inventoryGroup.panel_PlayerStatus;
        List<BodyPart> tempBodyParts = new List<BodyPart>();

        // ��ü ���� �� �ִ� ü�� ���� ���� ���� ��������.

        // �Ӹ�
        BodyPart head = new BodyPart(BodyPartTypes.Head, ssm.bodyPartSystem.HeadMaxHP_Init);
        head.Activate.Subscribe(Dead);
        tempBodyParts.Add(head);
        // UI����
        playerStatusUI.head.initMaxHp = ssm.bodyPartSystem.HeadMaxHP_Init;
        /// ����
        head.Hp.Subscribe(playerStatusUI.head.UpdateHP_View);
        head.CurrentMaxHp.Subscribe(playerStatusUI.head.UpdateCurrentMaxHP_View);
        head.Hp.Subscribe(CalculateCurrentHPSum);
        head.CurrentMaxHp.Subscribe(CalculateCurrentMaxHPSum);
        /// ������Ʈ
        playerStatusUI.head.UpdateHP_View(head.Hp.Value);
        playerStatusUI.head.UpdateCurrentMaxHP_View(head.CurrentMaxHp.Value);
        //


        // ����
        BodyPart leftArm = new BodyPart(BodyPartTypes.LeftArm, ssm.bodyPartSystem.ArmMaxHP_Init);
        leftArm.Activate.Subscribe(Debuff_CraftSpeed);
        tempBodyParts.Add(leftArm);
        // UI����
        playerStatusUI.leftArm.initMaxHp = ssm.bodyPartSystem.ArmMaxHP_Init;
        /// ����
        leftArm.Hp.Subscribe(playerStatusUI.leftArm.UpdateHP_View);
        leftArm.CurrentMaxHp.Subscribe(playerStatusUI.leftArm.UpdateCurrentMaxHP_View);
        leftArm.Hp.Subscribe(CalculateCurrentHPSum);
        leftArm.CurrentMaxHp.Subscribe(CalculateCurrentMaxHPSum);
        /// ������Ʈ
        playerStatusUI.leftArm.UpdateHP_View(leftArm.Hp.Value);
        playerStatusUI.leftArm.UpdateCurrentMaxHP_View(leftArm.CurrentMaxHp.Value);
        //


        // ������
        BodyPart rightArm = new BodyPart(BodyPartTypes.RightArm, ssm.bodyPartSystem.ArmMaxHP_Init);
        rightArm.Activate.Subscribe(Debuff_LockWeaponUse);
        tempBodyParts.Add(rightArm);
        // UI����
        playerStatusUI.rightArm.initMaxHp = ssm.bodyPartSystem.ArmMaxHP_Init;
        /// ����
        rightArm.Hp.Subscribe(playerStatusUI.rightArm.UpdateHP_View);
        rightArm.CurrentMaxHp.Subscribe(playerStatusUI.rightArm.UpdateCurrentMaxHP_View);
        rightArm.Hp.Subscribe(CalculateCurrentHPSum);
        rightArm.CurrentMaxHp.Subscribe(CalculateCurrentMaxHPSum);
        /// ������Ʈ
        playerStatusUI.rightArm.UpdateHP_View(rightArm.Hp.Value);
        playerStatusUI.rightArm.UpdateCurrentMaxHP_View(rightArm.CurrentMaxHp.Value);
        //


        // �޴ٸ�
        BodyPart leftLeg = new BodyPart(BodyPartTypes.LeftLeg, ssm.bodyPartSystem.LegMaxHP_Init);
        leftLeg.Activate.Subscribe(Debuff_SprintSpeed);
        tempBodyParts.Add(leftLeg);
        // UI����
        playerStatusUI.leftLeg.initMaxHp = ssm.bodyPartSystem.LegMaxHP_Init;
        /// ����
        leftLeg.Hp.Subscribe(playerStatusUI.leftLeg.UpdateHP_View);
        leftLeg.CurrentMaxHp.Subscribe(playerStatusUI.leftLeg.UpdateCurrentMaxHP_View);
        leftLeg.Hp.Subscribe(CalculateCurrentHPSum);
        leftLeg.CurrentMaxHp.Subscribe(CalculateCurrentMaxHPSum);
        /// ������Ʈ
        playerStatusUI.leftLeg.UpdateHP_View(leftLeg.Hp.Value);
        playerStatusUI.leftLeg.UpdateCurrentMaxHP_View(leftLeg.CurrentMaxHp.Value);
        //


        // �����ٸ�
        BodyPart rightLeg = new BodyPart(BodyPartTypes.RightLeg, ssm.bodyPartSystem.LegMaxHP_Init);
        rightLeg.Activate.Subscribe(Debuff_SprintSpeed);
        tempBodyParts.Add(rightLeg);
        // UI����
        playerStatusUI.rightLeg.initMaxHp = ssm.bodyPartSystem.LegMaxHP_Init;
        /// ����
        rightLeg.Hp.Subscribe(playerStatusUI.rightLeg.UpdateHP_View);
        rightLeg.CurrentMaxHp.Subscribe(playerStatusUI.rightLeg.UpdateCurrentMaxHP_View);
        rightLeg.Hp.Subscribe(CalculateCurrentHPSum);
        rightLeg.CurrentMaxHp.Subscribe(CalculateCurrentMaxHPSum);
        /// ������Ʈ
        playerStatusUI.rightLeg.UpdateHP_View(rightLeg.Hp.Value);
        playerStatusUI.rightLeg.UpdateCurrentMaxHP_View(rightLeg.CurrentMaxHp.Value);
        //

        
        // ��ü ������ �ʱ�ȭ
        bodyParts = tempBodyParts;

        // ü�� �ջ� ��ġ UI����
        SumCurrentHP.Subscribe(playerStatusUI.state_HpSum.UpdateStateNumb_View);
        SumCurrentMaxHP.Subscribe(playerStatusUI.state_HpSum.UpdateMaxStateNumb_View);
        playerStatusUI.state_HpSum.initMax = ssm.GetInitBodyPartsHPSum();

        // ���͸� ��ġ UI ����
        CurrentBattery.Subscribe(playerStatusUI.state_Battery.UpdateStateNumb_View);
        MaxBattery.Subscribe(playerStatusUI.state_Battery.UpdateMaxStateNumb_View);
        playerStatusUI.state_Battery.initMax = ssm.batterySystem.MaxBattery_Init;

        // ���ŷ� ��ġ UI ����
        CurrentWillPower.Subscribe(playerStatusUI.state_WillPower.UpdateStateNumb_View);
        playerStatusUI.state_WillPower.UpdateMaxStateNumb_View(ssm.willPowerSystem.MaxWillPower_Init);

        CalculateCurrentHPSum(0);
        CalculateCurrentMaxHPSum(0);
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
        for (int i = 0; i < bodyParts.Count; i++)
        {
            switch (bodyParts[i].type)
            {
                case BodyPartTypes.Head:
                    bodyParts[i].CurrentMaxHp.Value = ssm.bodyPartSystem.HeadMaxHP_AfterDestroyed;
                    break;

                case BodyPartTypes.LeftArm:
                case BodyPartTypes.RightArm:
                    bodyParts[i].CurrentMaxHp.Value = ssm.bodyPartSystem.ArmMaxHP_AfterDestroyed;
                    break;

                case BodyPartTypes.LeftLeg:
                case BodyPartTypes.RightLeg:
                    bodyParts[i].CurrentMaxHp.Value = ssm.bodyPartSystem.LegMaxHP_AfterDestroyed;
                    break;
            }

            bodyParts[i].Hp = bodyParts[i].CurrentMaxHp;
        }

    }

    public void InitBattery()
    {
        SuvivalSystemManager ssm = SuvivalSystemManager.Instance;

        MaxBattery.Value = ssm.batterySystem.MaxBattery_Init;
        CurrentBattery.Value = MaxBattery.Value;
    }

    public void ChargeBattery(float amount)
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

    public void Dispose()
    {
        CurrentWillPower.UnsbscribeAll();
        CurrentBattery.UnsbscribeAll();
        MaxBattery.UnsbscribeAll();
        SumCurrentHP.UnsbscribeAll();
        SumCurrentMaxHP.UnsbscribeAll();
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