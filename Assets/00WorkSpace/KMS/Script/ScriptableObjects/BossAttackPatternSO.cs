using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAttackShape { Circle, Cone, Box }

[System.Serializable]
public class BossAttackPattern
{
    public string patternName;
    public AnimationClip preludeAnimation; // 전조
    public AnimationClip attackAnimation;  // 본공격
    public float preludeTime;              // 전조 모션 시간
    public float afterDelay;               // 후딜, 후방 경직 시간
    public BossAttackShape shape;
    public float damage;
    public float range;
    public float angle;                    // 원뿔형 공격에만
    public float width, height, length;    // 박스형만
    public float cooldown;
}

[CreateAssetMenu(menuName = "Monster/BossAttackPatternSO")]
public class BossAttackPatternSO : ScriptableObject
{
    public List<BossAttackPattern> patterns;
}
