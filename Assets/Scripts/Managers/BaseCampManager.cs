using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    // 참조한 다른 매니저들보다 후 순위에서 초기화 해야 함
    PlayerManager pm;
    public Temp_BaseCampData baseCampData;

    public int MaxLevel { get; private set; }
    public BaseCampLevelUpCondition[] levelUpConditions;

    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
        baseCampData = new Temp_BaseCampData();
    }

    private void Start()
    {
        InitLevelUpConditions();
    }

    // Recoureces에서 베이스캠프 레벨업 조건(스크립터블obj) 배열로 받아와 레벨 순으로 정렬하기
    private void InitLevelUpConditions()
    {
        levelUpConditions = Resources.LoadAll<BaseCampLevelUpCondition>("BaseCampLevelUpConditions");
        Array.Sort(levelUpConditions, (a, b) => a.currentLevel.CompareTo(b.currentLevel));
    }

}
