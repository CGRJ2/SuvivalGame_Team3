using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BaseCampUpgradeCondition", menuName = "New BaseCampUpgradeCondition")]

public class BaseCampUpgradeCondition : ScriptableObject
{
    [Header("현재 레벨 조건(업그레이드 이후의 레벨 아님!)")]
    public int currentLevel;

    [Header("해당 업그레이드 시 베이스 캠프의 변화할 모습")]
    public Sprite baseCampSprite;

    [Header("업그레이드 진행 시간")]
    public float upgradingTime;

    [Header("조건: 스테이지 해금 여부")]
    public StageData needUnlockStage;

    [Header("조건: 아이템 보유 여부")]
    public List<ItemRequirement> requiredItems;

}
