using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/BossRushPatternSO")]
public class BossRushPatternSO : ScriptableObject, IAttackPattern
{
    public string patternName;
    public AnimationClip rushAnimation;
    public float rushSpeed;
    public float rushDistance;
    private int rushDamage;
    private float rushCooldown;
    public int Damage => rushDamage;
    public float Cooldown => rushCooldown;
}
