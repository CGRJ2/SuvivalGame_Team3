using System.Collections.Generic;
using UnityEngine;

public enum MonsterType { Normal, Boss, Cat, Onwer } // 메인 타입
public enum MonsterSubType { Balance, Sensor, Tank, Nuker, Assassin } // 기획안 적용
public enum MonsterTargetType { Player, Ally, None }
// Player = 플레이어
// Ally = 도우미(있을 경우 추적). 없을 경우: 플레이어 앞의 오브젝트를 추적하여 예상 이동 경로로 추적
// None = 타겟 없음 (방어물, 아이템형 몬스터 등에서 사용)

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class BaseMonsterData : ScriptableObject
{
    [Header("무적몬스터인지 아닌지 여부")]
    public bool isInvinvibleMonster;

    [Header("기본 정보")]
    public string monsterName;
    public MonsterType monsterType;
    public MonsterSubType monsterSubType;
    public MonsterTypeStatData typeStatData;

    [Header("기본 스탯")]
    [SerializeField] private float maxHP;
    [SerializeField] private float moveSpeed;

    [Header("공격 및 충돌 데미지")]
    [SerializeField] private float attackPower;
    [SerializeField] private int collisionDamage;

    [Header("공격 범위 및 쿨타임")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;

    [Header("죽음상태 이후 파괴까지 걸리는 시간")]
    public float destroyDelayTime = 1;

    [field: Header("넉백 관련 수치 설정(경직시간 & 넉백 힘)")]
    [field: SerializeField] public float HitStunDuration { get; private set; }
    [field: SerializeField] public float KnockBackedPower { get; private set; }

    [Header("공격 애니메이션 속도 배율")]
    [SerializeField] private float attackAnimSpeed = 1f; // (1.0이 기본, 1.2 빠름, 0.8 느림 등)

    [Header("탐지")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float baseFOV = 40f;
    [SerializeField] private float eyeHeight = 1.5f;
    [SerializeField] private MonsterTargetType targetType;
    [SerializeField] private float actionRadius = 20f;

    [Header("Idle 대기시간 랜덤 범위")]
    public float WaitTimeMin = 1f;
    public float WaitTimeMax = 4f;




    public float MaxHP => maxHP;
    public float MoveSpeed => moveSpeed;
    public float AttackPower => attackPower;
    public int CollisionDamage => collisionDamage;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public float DetectionRange => detectionRange;
    public float BaseFOV => baseFOV;
    public float EyeHeight => eyeHeight;
    public MonsterTargetType TargetType => targetType;
    public float ActionRadius => actionRadius;
    public float AttackAnimSpeed => attackAnimSpeed;


    [Header("프리팹")]
    public GameObject dropItemPrefab;
    public GameObject monsterPrefab;

    [Header("기타")]
    public Vector3 spawnPosition;
    public bool useFixedPosition; //보스나 주인 리스폰설정
}
