using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(HitInfo hitInfo);
}

public struct HitInfo
{
    public Transform Attacker;
    public Vector3 HitPoint;
    public Vector3 HitNormal;
    public float Damage;
    public float KnockbackPower;
    public float HitStun;       // 경직 시간
    public float IFrame;        // 피격 후 무적 시간
}