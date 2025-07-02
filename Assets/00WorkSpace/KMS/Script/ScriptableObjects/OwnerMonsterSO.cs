using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/OwnerMonsterSO")]
public class OwnerMonsterSO : BaseMonsterData
{
    [Header("���� ���� �ɼ�")]
    public float moveSpeed = 4f;                   // �̵� �ӵ�
    public float detectionRange = 6f;              // ���� ����

    [Header("���� ���� ��Ÿ")]
    public List<GameObject> preferredBaitItems;    // ����� ����ȭ ������
}
