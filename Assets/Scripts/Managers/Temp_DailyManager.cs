using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Temp_DailyManager : Singleton<Temp_DailyManager>
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
    [field: SerializeField] public float CurrentTime { get; private set; }
    [field: SerializeField] public ObservableProperty<int> CurrentDay { get; private set; }
    [field: SerializeField] public ObservableProperty<TimeZoneState> TZ_State { get; private set; }
    
    


    public void Init()
    {
        base.SingletonInit();
    }

    public void InitDay()
    {
        CurrentDay.Value = 1;
    }

    public void InitTime()
    {
        CurrentTime = 0;
    }

    public void LoadData(float currentTime, int currentDay)
    {
        this.CurrentDay.Value = currentDay;
        this.CurrentTime = currentTime;
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
            CurrentTime += Time.deltaTime;


            if (CurrentTime <= timeZoneSetting.TZ_Morning)
            {
                TZ_State.Value = TimeZoneState.Morning;
            }
            else if (CurrentTime <= timeZoneSetting.TZ_Afternoon)
            {
                TZ_State.Value = TimeZoneState.Afternoon;
            }
            else if (CurrentTime <= timeZoneSetting.TZ_Evening)
            {
                TZ_State.Value = TimeZoneState.Evening;
            }
            else if (CurrentTime <= timeZoneSetting.DayCycleTime)
            {
                TZ_State.Value = TimeZoneState.Night;
            }
            else if (CurrentTime > timeZoneSetting.DayCycleTime)
            {
                CurrentTime = 0;
                MoveToNextDay();
            }
        }
    }


    public void MoveToNextDay()
    {
        if (CurrentDay.Value < timeZoneSetting.MaxDayCount)
        {
            CurrentDay.Value++;
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
    Morning, Afternoon, Evening, Night
}