using System;
using Unity.VisualScripting;

[System.Serializable]
public class BodyPart : IDisposable
{
    public BodyPartTypes type;
    // 활성화 상태, 비활성화 시 체력회복 불가능, 1차회복 시 다시 활성화
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
   
    // 파츠 정보 초기화 & 파츠 교체 시 호출 ==> 교체하면 현재체력 & 최대내구도까지 원상복귀
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

    // 회복 효과 => 현재 최대내구도 까지만 회복 가능
    public void Repair(float amount)
    {
        if (Activate.Value == false) return;

        if (Hp.Value + amount > CurrentMaxHp.Value) Hp.Value = CurrentMaxHp.Value;
        else Hp.Value += amount;
    }

    // 회복 가능 상태로 만듦
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
