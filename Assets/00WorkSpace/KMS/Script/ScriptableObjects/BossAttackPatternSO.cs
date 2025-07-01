using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAttackShape { Circle, Cone, Box }

[System.Serializable]
public class BossAttackPattern
{
    public string patternName;
    public BossAttackShape shape;
    public float damage;
    public float range;
    public float angle; // 원뿔형 공격에만
    public float width, height, length; // 박스형만
    public float cooldown;
    public AnimationClip attackAnimation;
    //필요한 것 추가 가능
}

[CreateAssetMenu(menuName = "Monster/BossAttackPatternSO")]
public class BossAttackPatternSO : ScriptableObject
{
    public List<BossAttackPattern> patterns;
}
