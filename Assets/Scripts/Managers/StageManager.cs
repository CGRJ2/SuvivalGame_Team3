using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [HideInInspector] public StageData[] stageDatas;
    public ObservableProperty<int> CurrentStageLevel = new ObservableProperty<int>();


    public void Init()
    {
        base.SingletonInit();

        // 스테이지 정보 초기화
        InitStageDatas(); 
        

        // 스테이지 정보 불러오기 & 클리어 정보 => 현재 스테이지 레벨에 동기화
        //LoadStageUnlockSaveData(데이터 매니저에서 선택된 세이브 데이터 안에 있는 스테이지언락여부 리스트 받아오기)

    }

    private void OnDestroy()
    {
        CurrentStageLevel.UnsbscribeAll();

    }
    
    // 스테이지 데이터들 초기화하기
    private void InitStageDatas()
    {
        stageDatas = Resources.LoadAll<StageData>("StageDatas");
        Array.Sort(stageDatas, (a, b) => a.StageLevel.CompareTo(b.StageLevel));

        for (int i = 0; i < stageDatas.Length; i++)
        {
            stageDatas[i].Init();
        }
    }


    // 스테이지가 해금될 때마다 CurrentStageLevel 업데이트
    public void SetCurrentStageIndex(int stageLevel)
    {
        CurrentStageLevel.Value = stageLevel;
    }



    #region [세이브 & 로드 데이터]

    public List<bool> GetStageUnlockSaveData()
    {
        // 언락 여부만 리스트 순서대로 저장
        List<bool> instanceList = new List<bool>();
        foreach (StageData stageData in stageDatas)
        {
            instanceList.Add(stageData.IsUnlocked);
        }
        return instanceList;
    }

    public void LoadStageUnlockSaveData(List<bool> loadedData)
    {
        for (int i = 0; i < stageDatas.Length; i++)
        {
            if (loadedData[i]) stageDatas[i].UlockStage();
        }
    }

    #endregion

}

public enum StageKey
{
    거실, 서재, 옷방, 안방, All
}


