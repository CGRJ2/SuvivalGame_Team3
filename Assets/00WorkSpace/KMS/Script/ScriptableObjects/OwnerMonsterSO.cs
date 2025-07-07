using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/OwnerMonsterSO")]
public class OwnerMonsterSO : BaseMonsterData
{
    [Header("주인 전용 기타")]
    public List<GameObject> preferredBaitItems;    // 레고등 무력화 아이템
}
