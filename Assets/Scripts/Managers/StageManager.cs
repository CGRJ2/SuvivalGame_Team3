using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [HideInInspector] public StageData[] stageDatas;
    public ObservableProperty<int> CurrentStageLevel = new ObservableProperty<int>();

    public void Init()
    {
        base.SingletonInit();

        SortStageDatasByLevel();
    }

    private void OnDestroy()
    {
        CurrentStageLevel.UnsbscribeAll();

    }

    // ���������� �رݵ� ������ CurrentStageLevel ������Ʈ
    public void SetCurrentStageIndex(int stageLevel)
    {
        CurrentStageLevel.Value = stageLevel;
    }

    private void SortStageDatasByLevel()
    {
        stageDatas = Resources.LoadAll<StageData>("StageDatas");
        Array.Sort(stageDatas, (a, b) => a.StageLevel.CompareTo(b.StageLevel));
    }

}
