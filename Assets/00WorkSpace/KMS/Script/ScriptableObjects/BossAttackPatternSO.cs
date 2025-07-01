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
    public float angle; // ������ ���ݿ���
    public float width, height, length; // �ڽ�����
    public float cooldown;
    public AnimationClip attackAnimation;
    //�ʿ��� �� �߰� ����
}

[CreateAssetMenu(menuName = "Monster/BossAttackPatternSO")]
public class BossAttackPatternSO : ScriptableObject
{
    public List<BossAttackPattern> patterns;
}
