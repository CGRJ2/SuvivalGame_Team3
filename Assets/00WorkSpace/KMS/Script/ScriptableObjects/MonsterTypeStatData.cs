using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/TypeStatData", fileName = "MonsterTypeStatData")]
public class MonsterTypeStatData : ScriptableObject
{
    [Header("�⺻ ����")]
    public MonsterSubType subType;

    [Header("���� �ɷ�ġ ���")]
    public float attackPowerMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public float attackRangeMultiplier = 1f;
    public float collisionDamageMultiplier = 1f;

    [Header("�˹� ����")]
    public float knockbackDistanceMultiplier = 1f;

    [Header("AI �� �̵� ����")]
    public float moveSpeedMultiplier = 1f;
    public float hpMultiplier = 1f;
    public float detectionRangeMultiplier = 1f;

    [Header("����")]
    [TextArea] public string description;
    [TextArea] public string designIntent;
}
