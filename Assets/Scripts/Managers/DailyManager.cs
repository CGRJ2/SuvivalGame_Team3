using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DailyManager : Singleton<DailyManager>
{
    [System.Serializable]
    private class TimeZoneSetting
    {
        [field: Header("�Ϸ� �� �ð�")]
        [field: SerializeField] public float DayCycleTime { get; private set; }
        [field: Header("�ִ� ��¥ (���� ����)")]
        [field: SerializeField] public int MaxDayCount { get; private set; }

        [field: Header("�ð��� ���� ����")]
        [field: SerializeField] public float TZ_Morning { get; private set; }
        [field: SerializeField] public float TZ_Afternoon { get; private set; }
        [field: SerializeField] public float TZ_Evening { get; private set; }

        
    }

    [Header("�ð��� ���� ����")]
    [SerializeField] TimeZoneSetting timeZoneSetting;

    [field: Header("���� �ð� ����")]
    public CurrentTimeData currentTimeData = new CurrentTimeData();

    


    public void Init()
    {
        base.SingletonInit();

        // ������ �ε��� �� currentTimeData�� �ε��� �����ͷ� ��ü
        DataManager dm = DataManager.Instance;
        dm.loadedDataGroup.Subscribe(LoadTimeData);
    }

    public void InitDay()
    {
        currentTimeData.CurrentDay.Value = 1;
    }

    public void InitTime()
    {
        currentTimeData.CurrentTime = 0;
    }

    private void LoadTimeData(SaveDataGroup saveDataGroup)
    {
        currentTimeData = saveDataGroup.currentTimeData;
    }

    private void Start()
    {
        // �׽�Ʈ �뵵
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return null;
            currentTimeData.CurrentTime += Time.deltaTime;


            if (currentTimeData.CurrentTime <= timeZoneSetting.TZ_Morning)
            {
                currentTimeData.TZ_State.Value = TimeZoneState.Morning;
            }
            else if (currentTimeData.CurrentTime <= timeZoneSetting.TZ_Afternoon)
            {
                currentTimeData.TZ_State.Value = TimeZoneState.Afternoon;
            }
            else if (currentTimeData.CurrentTime <= timeZoneSetting.TZ_Evening)
            {
                currentTimeData.TZ_State.Value = TimeZoneState.Evening;
            }
            else if (currentTimeData.CurrentTime <= timeZoneSetting.DayCycleTime)
            {
                currentTimeData.TZ_State.Value = TimeZoneState.Night;
            }
            else if (currentTimeData.CurrentTime > timeZoneSetting.DayCycleTime)
            {
                currentTimeData.CurrentTime = 0;
                MoveToNextDay();
            }
        }
    }


    public void MoveToNextDay()
    {
        if (currentTimeData.CurrentDay.Value < timeZoneSetting.MaxDayCount)
        {
            currentTimeData.CurrentDay.Value++;
        }
        else
        {
            Debug.Log("�������� ���� => ���� ����");
        }
    }

    public void DayStart()
    {

    }

}

public enum TimeZoneState
{
    Morning, Afternoon, Evening, Night, All
}

[System.Serializable]
public class CurrentTimeData
{
    public float CurrentTime;
    public ObservableProperty<int> CurrentDay;
    public ObservableProperty<TimeZoneState> TZ_State;
}