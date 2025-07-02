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
    [Tooltip("Circle ������ ������ ����, Cone ������ ���̷� ������ּ���.")]
    public float range;
    [Tooltip("Cone ���Ͽ��� ����ϼ���")]
    public float angle;           // ������ ����
    [Tooltip("Box ���Ͽ��� ����ϼ���")]
    public float width, height, length; // �ڽ���
    public float cooldown;
}

[CreateAssetMenu(menuName = "Monster/BossAttackPatternSO")]
public class BossAttackPatternSO : ScriptableObject
{
    public List<BossAttackPattern> patterns;
}

