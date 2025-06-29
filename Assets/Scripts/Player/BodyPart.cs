using System;
using Unity.VisualScripting;

[System.Serializable]
public class BodyPart : IDisposable
{
    public BodyPartTypes type;
    // Ȱ��ȭ ����, ��Ȱ��ȭ �� ü��ȸ�� �Ұ���, 1��ȸ�� �� �ٽ� Ȱ��ȭ
    public ObservableProperty<bool> Activate = new ObservableProperty<bool>(); 
    public ObservableProperty<int> Hp = new ObservableProperty<int>();
    public ObservableProperty<int> CurrentMaxHp = new ObservableProperty<int>();
    private int InitMaxHp;
    private Action<bool> deactiveEffectAction;

    public BodyPart(BodyPartTypes type, int maxHp, Action<bool> DeactiveEffectAction)
    {
        Activate.Subscribe(DeactiveEffect);
        
        this.type = type;
        this.InitMaxHp = maxHp;
        this.deactiveEffectAction = DeactiveEffectAction;

        Init();
    }
   
    // ���� ���� �ʱ�ȭ & ���� ��ü �� ȣ�� ==> ��ü�ϸ� ����ü�� & �ִ볻�������� ���󺹱�
    public void Init()
    {
        Hp.Value = InitMaxHp;
        CurrentMaxHp.Value = InitMaxHp;

        Activate.Value = true;
    }

    public void TakeDamage(int damage)
    {
        if (Hp.Value - damage <= 0)
        {
            Hp.Value = 0;
            Activate.Value = false;
        }
        else Hp.Value -= damage;
    }

    // ȸ�� ȿ�� => ���� �ִ볻���� ������ ȸ�� ����
    public void Repair(int amount)
    {
        if (Activate.Value == false) return;

        if (Hp.Value + amount > CurrentMaxHp.Value) Hp.Value = CurrentMaxHp.Value;
        else Hp.Value += amount;
    }

    // ȸ�� ���� ���·� ����
    public void QuickRepair(int maxHP_AfterQuickRepair)
    {
        Hp.Value = 1;
        CurrentMaxHp.Value = maxHP_AfterQuickRepair;
        Activate.Value = true;
    }

    private void DeactiveEffect(bool isActive)
    {
        if (!isActive)
            deactiveEffectAction?.Invoke(isActive);
    }

    public void Dispose()
    {
        Activate.Unsubscribe(DeactiveEffect);
    }
}
