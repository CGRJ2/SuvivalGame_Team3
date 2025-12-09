using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(HitInfo hitInfo);
}

public struct HitInfo
{
    public float Damage;
    public Vector3 KnockbackDir;
    public float KnockbackPower;
    public Transform Attacker;
}