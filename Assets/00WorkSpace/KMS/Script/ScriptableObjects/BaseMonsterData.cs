using UnityEngine;

public enum MonsterType { Normal, Boss, Cat, Onwer } // 메인 타입
public enum MonsterSubType { Toy1, Toy2, Doll1, Doll2 } //임의 배정
public enum MonsterTargetType { Player, Ally, None }
// Player = 플레이어
// Ally = 도우미(있을 경우 추적). 없을 경우: 플레이어 앞의 오브젝트를 추적하여 예상 이동 경로로 추적
// None = 타겟 없음 (방어물, 아이템형 몬스터 등에서 사용)

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class BaseMonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public string monsterName;
    public MonsterType monsterType;
    public MonsterSubType monsterSubType;

    [Header("기본 스탯")]
    [SerializeField] private float maxHP;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;

    [Header("탐지")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float baseFOV = 40f;
    [SerializeField] private float eyeHeight = 1.5f;
    [SerializeField] private MonsterTargetType targetType;
    [SerializeField] private float actionRadius = 20f;

    public float MaxHP => maxHP;
    public float MoveSpeed => moveSpeed;
    public float AttackPower => attackPower;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;

    public float DetectionRange => detectionRange;
    public float BaseFOV => baseFOV;
    public float EyeHeight => eyeHeight;
    public MonsterTargetType TargetType => targetType;
    public float ActionRadius => actionRadius;
    [Header("프리팹")]
    public GameObject dropItemPrefab;
    public GameObject monsterPrefab;

    [Header("기타")]
    public Vector3 spawnPosition;
    public bool useFixedPosition; //보스나 주인 리스폰설정
}
