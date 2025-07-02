using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAttackShape { Circle, Cone, Box }
public enum CircleOriginType { Boss, Player }

[System.Serializable]
public class BossAttackPattern
{
    public string patternName;
    public AnimationClip preludeAnimation;
    public AnimationClip attackAnimation;
    public BossAttackShape shape;
    public float damage;
    public CircleOriginType originType;
    [Tooltip("Circle 패턴의 반지름 길이, Cone 패턴의 길이로 사용해주세요.")]
    public float range;
    [Tooltip("Cone 패턴에만 사용하세요")]
    public float angle;           // 원뿔형 공격
    [Tooltip("Box 패턴에만 사용하세요")]
    public float width, height, length; // 박스형
    public float cooldown;
}

[CreateAssetMenu(menuName = "Monster/BossAttackPatternSO")]
public class BossAttackPatternSO : ScriptableObject
{
    public List<BossAttackPattern> patterns;
}

