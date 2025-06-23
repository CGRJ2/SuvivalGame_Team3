using UnityEngine;

public enum MonsterType { Normal, Elite, Boss }
public enum MonsterTargetType { Player, Ally, None }

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class BaseMonsterData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string monsterName;
    public MonsterType monsterType;
    public float maxHP;
    public float moveSpeed;
    public float attackCooldown;
    public float attackPower;

    [Header("Ž��")]
    public float detectionRange;
    public MonsterTargetType targetType;

    [Header("��Ÿ")]
    public GameObject dropItemPrefab;
    public GameObject prefab;
}
