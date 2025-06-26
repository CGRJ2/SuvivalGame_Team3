using UnityEngine;

public enum MonsterType { Normal, Boss, Cat, Onwer } // ���� Ÿ��
public enum MonsterSubType { Toy1, Toy2, Doll1, Doll2 } //���� ����
public enum MonsterTargetType { Player, Ally, None }
// Player = �÷��̾�
// Ally = �����(���� ��� ����). ���� ���: �÷��̾� ���� ������Ʈ�� �����Ͽ� ���� �̵� ��η� ����
// None = Ÿ�� ���� (��, �������� ���� ��� ���)

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class BaseMonsterData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string monsterName;
    public MonsterType monsterType;
    public MonsterSubType monsterSubType;

    [Header("�⺻ ����")]
    [SerializeField] private float maxHP;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;

    [Header("Ž��")]
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
    [Header("������")]
    public GameObject dropItemPrefab;
    public GameObject monsterPrefab;

    [Header("��Ÿ")]
    public Vector3 spawnPosition;
    public bool useFixedPosition; //������ ���� ����������
}
