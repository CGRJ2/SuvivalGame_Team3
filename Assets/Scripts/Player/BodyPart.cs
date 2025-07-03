using System;
using Unity.VisualScripting;

[System.Serializable]
public class BodyPart : IDisposable
{
    public BodyPartTypes type;
    // Ȱ��ȭ ����, ��Ȱ��ȭ �� ü��ȸ�� �Ұ���, 1��ȸ�� �� �ٽ� Ȱ��ȭ
    public ObservableProperty<bool> Activate = new ObservableProperty<bool>(); 
    public int Hp;
    public int CurrentMaxHp;
    private int InitMaxHp;

    public BodyPart(BodyPartTypes type, int maxHp)
    {
        this.type = type;
        this.InitMaxHp = maxHp;

        Init();
    }
   
    // ���� ���� �ʱ�ȭ & ���� ��ü �� ȣ�� ==> ��ü�ϸ� ����ü�� & �ִ볻�������� ���󺹱�
    public void Init()
    {
        Hp = InitMaxHp;
        CurrentMaxHp = InitMaxHp;

        Activate.Value = true;
    }

    public void TakeDamage(int damage)
    {
        if (Hp - damage <= 0)
        {
            Hp = 0;
            Activate.Value = false;
        }
        else Hp -= damage;
    }

    // ȸ�� ȿ�� => ���� �ִ볻���� ������ ȸ�� ����
    public void Repair(int amount)
    {
        if (Activate.Value == false) return;

        if (Hp + amount > CurrentMaxHp) Hp = CurrentMaxHp;
        else Hp += amount;
    }

    // ȸ�� ���� ���·� ����
    public void QuickRepair(int maxHP_AfterQuickRepair)
    {
        Hp = 1;
        CurrentMaxHp = maxHP_AfterQuickRepair;
        Activate.Value = true;
    }

    public void Dispose()
    {
        Activate.UnsbscribeAll();
    }
}
