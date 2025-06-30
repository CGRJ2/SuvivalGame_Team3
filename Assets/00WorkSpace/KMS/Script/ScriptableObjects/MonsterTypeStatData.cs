using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/TypeStatData", fileName = "MonsterTypeStatData")]
public class MonsterTypeStatData : ScriptableObject
{
    [Header("기본 설정")]
    public MonsterSubType subType;

    [Header("전투 능력치 배수")]
    public float attackPowerMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public float attackRangeMultiplier = 1f;
    public float collisionDamageMultiplier = 1f;

    [Header("넉백 관련")]
    public float knockbackDistanceMultiplier = 1f;

    [Header("AI 및 이동 관련")]
    public float moveSpeedMultiplier = 1f;
    public float hpMultiplier = 1f;
    public float detectionRangeMultiplier = 1f;

    [Header("설명")]
    [TextArea] public string description;
    [TextArea] public string designIntent;
}
