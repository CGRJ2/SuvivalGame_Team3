using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/OwnerMonsterSO")]
public class OwnerMonsterSO : BaseMonsterData
{
    [Header("주인 전용 옵션")]
    public float moveSpeed = 4f;
    public float detectionRange = 6f;

    [Header("주인 전용 기타")]
    public List<GameObject> preferredBaitItems;
}
