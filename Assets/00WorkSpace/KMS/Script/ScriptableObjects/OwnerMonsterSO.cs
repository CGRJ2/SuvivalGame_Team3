using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/OwnerMonsterSO")]
public class OwnerMonsterSO : BaseMonsterData
{
    [Header("���� ���� ��Ÿ")]
    public List<GameObject> preferredBaitItems;    // ����� ����ȭ ������
}
