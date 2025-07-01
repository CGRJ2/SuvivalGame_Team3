using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAttackShape { Circle, Cone, Box }

[System.Serializable]
public class BossAttackPattern
{
    public string patternName;
    public AnimationClip preludeAnimation; // ����
    public AnimationClip attackAnimation;  // ������
    public float preludeTime;              // ���� ��� �ð�
    public float afterDelay;               // �ĵ�, �Ĺ� ���� �ð�
    public BossAttackShape shape;
    public float damage;
    public float range;
    public float angle;                    // ������ ���ݿ���
    public float width, height, length;    // �ڽ�����
    public float cooldown;
}

[CreateAssetMenu(menuName = "Monster/BossAttackPatternSO")]
public class BossAttackPatternSO : ScriptableObject
{
    public List<BossAttackPattern> patterns;
}
