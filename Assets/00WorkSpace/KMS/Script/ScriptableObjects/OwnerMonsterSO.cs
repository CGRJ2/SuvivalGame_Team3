using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/OwnerMonsterSO")]
public class OwnerMonsterSO : BaseMonsterData
{
    [Header("주인 전용 옵션")]
    public float moveSpeed = 4f;                   // 이동 속도
    public float detectionRange = 6f;              // 감지 범위

    [Header("주인 전용 기타")]
    public List<GameObject> preferredBaitItems;    // 레고등 무력화 아이템
}
