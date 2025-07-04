using System;
using Unity.VisualScripting;

[System.Serializable]
public class BodyPart : IDisposable
{
    public BodyPartTypes type;
    // Ȱ��ȭ ����, ��Ȱ��ȭ �� ü��ȸ�� �Ұ���, 1��ȸ�� �� �ٽ� Ȱ��ȭ
    public ObservableProperty<bool> Activate = new ObservableProperty<bool>(); 
    public ObservableProperty<float> Hp = new ObservableProperty<float>();
    public ObservableProperty<float> CurrentMaxHp = new ObservableProperty<float>();
    private float InitMaxHp;

    public BodyPart(BodyPartTypes type, float maxHp)
    {
        this.type = type;
        this.InitMaxHp = maxHp;

        Init();
    }
   
    // ���� ���� �ʱ�ȭ & ���� ��ü �� ȣ�� ==> ��ü�ϸ� ����ü�� & �ִ볻�������� ���󺹱�
    public void Init()
    {
        Hp.Value = InitMaxHp;
        CurrentMaxHp.Value = InitMaxHp;

        Activate.Value = true;
    }

    public void TakeDamage(float damage)
    {
        if (Hp.Value - damage <= 0)
        {
            Hp.Value = 0;
            Activate.Value = false;
        }
        else Hp.Value -= damage;
    }

    // ȸ�� ȿ�� => ���� �ִ볻���� ������ ȸ�� ����
    public void Repair(float amount)
    {
        if (Activate.Value == false) return;

        if (Hp.Value + amount > CurrentMaxHp.Value) Hp.Value = CurrentMaxHp.Value;
        else Hp.Value += amount;
    }

    // ȸ�� ���� ���·� ����
    public void QuickRepair(float maxHP_AfterQuickRepair)
    {
        Hp.Value = 1;
        CurrentMaxHp.Value = maxHP_AfterQuickRepair;
        Activate.Value = true;
    }

    public void Dispose()
    {
        Activate.UnsbscribeAll();
        Hp.UnsbscribeAll();
        CurrentMaxHp.UnsbscribeAll();
    }
}
