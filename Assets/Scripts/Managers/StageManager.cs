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

        // �������� ���� �ʱ�ȭ
        InitStageDatas(); 
        

        // �������� ���� �ҷ����� & Ŭ���� ���� => ���� �������� ������ ����ȭ
        //LoadStageUnlockSaveData(������ �Ŵ������� ���õ� ���̺� ������ �ȿ� �ִ� ��������������� ����Ʈ �޾ƿ���)

    }

    private void OnDestroy()
    {
        CurrentStageLevel.UnsbscribeAll();

    }
    
    // �������� �����͵� �ʱ�ȭ�ϱ�
    private void InitStageDatas()
    {
        stageDatas = Resources.LoadAll<StageData>("StageDatas");
        Array.Sort(stageDatas, (a, b) => a.StageLevel.CompareTo(b.StageLevel));

        for (int i = 0; i < stageDatas.Length; i++)
        {
            stageDatas[i].Init();
        }
    }


    // ���������� �رݵ� ������ CurrentStageLevel ������Ʈ
    public void SetCurrentStageIndex(int stageLevel)
    {
        CurrentStageLevel.Value = stageLevel;
    }



    #region [���̺� & �ε� ������]

    public List<bool> GetStageUnlockSaveData()
    {
        // ��� ���θ� ����Ʈ ������� ����
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
    �Ž�, ����, �ʹ�, �ȹ�, All
}


