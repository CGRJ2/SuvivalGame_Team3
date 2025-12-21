using System;

[Serializable]
public class PlayerCombatModel
{
    private float _defaultDamage;
    private float _defaultKnockback;
    private float _defaultHitStun;

    public Item HandedItem; // 손(활성화된 퀵슬롯)에 장착 중인 아이템

    public void Init()
    {
        var initStatTable   = Manager.data.PlayerStatsTable;
        _defaultDamage      = initStatTable.FindByKey("Damage").InitValue;
        _defaultKnockback   = initStatTable.FindByKey("KnockBackForce").InitValue;
        _defaultHitStun     = 1f;
    }

    public AttackData GetAttackData()
    {
        AttackData data;

        if (HandedItem != null && HandedItem is Item_Weapon weapon)
        {
            data = new AttackData()
            {
                AttackType  = weapon.AttackType,
                Damage      = _defaultDamage + weapon.Damage,
                Knockback   = _defaultKnockback + weapon.Knockback,
                HitStun     = _defaultHitStun + weapon.HitStun
            };
        }
        else
        {
            data = new AttackData()
            {
                AttackType  = (AttackType)0,
                Damage      = _defaultDamage,
                Knockback   = _defaultKnockback,
                HitStun     = _defaultHitStun
            };
        }
        return data;
    }
}

public struct AttackData
{
    public AttackType AttackType;
    public float Damage;
    public float Knockback;
    public float HitStun;
}

public enum AttackType { Punch, Slash, Thrust, OverHead }
