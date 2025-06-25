using System;
using Unity.VisualScripting;

[System.Serializable]
public class BodyPart : IDisposable
{
    public BodyPartTypes type;
    // Ȱ��ȭ ����, ��Ȱ��ȭ �� ü��ȸ�� �Ұ���, 1��ȸ�� �� �ٽ� Ȱ��ȭ
    public ObservableProperty<bool> Activate = new ObservableProperty<bool>(); 
    public ObservableProperty<int> Hp = new ObservableProperty<int>();
    private int MaxHp;
    private Action<bool> deactiveEffectAction;

    public BodyPart(BodyPartTypes type, int maxHp, Action<bool> DeactiveEffectAction)
    {
        Activate.Subscribe(DeactiveEffect);
        
        this.type = type;
        this.MaxHp = maxHp;
        this.deactiveEffectAction = DeactiveEffectAction;

        Init();
    }
   
    // ���� ���� �ʱ�ȭ & ���� ��ü �� ȣ��
    public void Init()
    {
        Hp.Value = MaxHp;
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

    public void Repair(int amount)
    {
        if (Hp.Value + amount > MaxHp) Hp.Value = MaxHp;
        else Hp.Value += amount;
    }

    public void QuickRepair()
    {
        Hp.Value = 1;
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
