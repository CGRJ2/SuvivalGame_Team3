using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 고양이 전용 데이터 SO
[CreateAssetMenu(fileName = "CatMonsterSO", menuName = "Monster/CatMonsterSO")]
public class CatMonsterSO : BaseMonsterData
{
    [Header("고양이 전용 옵션")]
    public float footstepAlertValue = 10;           // 발소리 경계도 상승치
    public float footstepDetectionRange = 12f;      // 발걸음 감지 범위
    public float chaseMoveSpeed = 5f;               // 추적 이동 속도
    public float cutsceneDuration = 3f;             // 컷신 지속 시간
    public float basicMoveSpeed = 3f;                // 일반 이동 속도
    public float catDetectionRange = 7f;            // 감지 범위

    [Header("고양이 전용 기타")]
    public List<GameObject> preferredBaitItems;     // 유인 우선순위용 미끼 아이템
}