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
        [field: Header("하루 총 시간")]
        [field: SerializeField] public float DayCycleTime { get; private set; }
        [field: Header("최대 날짜 (엔딩 조건)")]
        [field: SerializeField] public int MaxDayCount { get; private set; }

        [field: Header("시간대 정보 설정")]
        [field: SerializeField] public float TZ_Morning { get; private set; }
        [field: SerializeField] public float TZ_Afternoon { get; private set; }
        [field: SerializeField] public float TZ_Evening { get; private set; }

        
    }

    [Header("시간대 기준 설정")]
    [SerializeField] TimeZoneSetting timeZoneSetting;

    [field: Header("현재 시간 정보")]
    public CurrentTimeData currentTimeData = new CurrentTimeData();

    


    public void Init()
    {
        base.SingletonInit();

        // 데이터 로드할 때 currentTimeData를 로드한 데이터로 교체
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
        // 테스트 용도
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
            Debug.Log("다음날이 없음 => 게임 오버");
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