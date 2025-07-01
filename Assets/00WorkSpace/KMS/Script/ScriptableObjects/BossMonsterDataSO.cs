using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/BossDataSO")]
public class BossMonsterDataSO : BaseMonsterData
{
    [Header("보스 전용(Phase별)")]
    [SerializeField] private int phase2AttackPower;
    [SerializeField] private int phase3AttackPower;
    [SerializeField] private float phase2KnockbackDistance;
    [SerializeField] private float phase3KnockbackDistance;
    [SerializeField] private float phase2AttackCooldown;
    [SerializeField] private float phase3AttackCooldown;
    [SerializeField] private float phase2AnimSpeed = 1f;
    [SerializeField] private float phase3AnimSpeed = 1f;
    public int Phase2AttackPower => phase2AttackPower;
    public int Phase3AttackPower => phase3AttackPower;
    public float Phase2KnockbackDistance => phase2KnockbackDistance;
    public float Phase3KnockbackDistance => phase3KnockbackDistance;
    public float Phase2AttackCooldown => phase2AttackCooldown;
    public float Phase3AttackCooldown => phase3AttackCooldown;
    public float Phase2AnimSpeed => phase2AnimSpeed;
    public float Phase3AnimSpeed => phase3AnimSpeed;
}
