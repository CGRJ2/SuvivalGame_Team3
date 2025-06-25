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
    public float maxHP;
    public float moveSpeed;
    public float attackCooldown;
    public float attackPower;

    [Header("Ž��")]
    public float detectionRange;
    public float baseFOV = 40f;
    public float eyeHeight = 1.5f;
    public MonsterTargetType targetType;

    [Header("������")]
    public GameObject dropItemPrefab;
    public GameObject prefab;

    [Header("��Ÿ")]
    public Vector3 spawnPosition;
    public bool useFixedPosition; //������ ���� ����������
}
