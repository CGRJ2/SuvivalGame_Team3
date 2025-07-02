using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� ���� ������ SO
[CreateAssetMenu(fileName = "CatMonsterSO", menuName = "Monster/CatMonsterSO")]
public class CatMonsterSO : BaseMonsterData
{
    [Header("����� ���� �ɼ�")]
    public float footstepAlertValue = 10;           // �߼Ҹ� ��赵 ���ġ
    public float footstepDetectionRange = 12f;      // �߰��� ���� ����
    public float chaseMoveSpeed = 5f;               // ���� �̵� �ӵ�
    public float cutsceneDuration = 3f;             // �ƽ� ���� �ð�
    public float basicMoveSpeed = 3f;                // �Ϲ� �̵� �ӵ�
    public float catDetectionRange = 7f;            // ���� ����

    [Header("����� ���� ��Ÿ")]
    public List<GameObject> preferredBaitItems;     // ���� �켱������ �̳� ������
}