using UnityEngine;

public enum MonsterType { Normal, Boss, Cat, Onwer } // 메인 타입
public enum MonsterSubType { Toy1, Toy2, Doll1, Doll2 } //임의 배정
public enum MonsterTargetType { Player, Ally, None }

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class BaseMonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public string monsterName;
    public MonsterType monsterType;
    public MonsterSubType monsterSubType;

    [Header("기본 스탯")]
    public float maxHP;
    public float moveSpeed;
    public float attackCooldown;
    public float attackPower;

    [Header("탐지")]
    public float detectionRange;
    public MonsterTargetType targetType;

    [Header("프리팹")]
    public GameObject dropItemPrefab;
    public GameObject prefab;

    [Header("기타")]
    public Vector3 spawnPosition;
    public bool useFixedPosition; //보스나 주인 리스폰설정
}
