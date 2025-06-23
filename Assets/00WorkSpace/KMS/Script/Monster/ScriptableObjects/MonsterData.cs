using UnityEngine;

public enum MonsterType { Normal, Elite, Boss }
public enum MonsterTargetType { Player, Ally, None }

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class BaseMonsterData : ScriptableObject
{
    [Header("±‚∫ª Ω∫≈»")]
    public string monsterName;
    public MonsterType monsterType;
    public float maxHP;
    public float moveSpeed;
    public float attackCooldown;
    public float attackPower;

    [Header("≈Ω¡ˆ")]
    public float detectionRange;
    public MonsterTargetType targetType;

    [Header("±‚≈∏")]
    public GameObject dropItemPrefab;
    public GameObject prefab;
}
