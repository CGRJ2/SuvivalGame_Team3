using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DailyManager : Singleton<DailyManager>
{
    [System.Serializable]
    public class TimeZoneSetting
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

    [Header("시간 단위로 변환")]
    [SerializeField] string timeToString;




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
        currentTimeData.CurrentDay.Value = saveDataGroup.currentTimeData.CurrentDay.Value;
        currentTimeData.CurrentTime = saveDataGroup.currentTimeData.CurrentTime;
        currentTimeData.TZ_State.Value = saveDataGroup.currentTimeData.TZ_State.Value;


        Debug.Log("데일리 매니저 구독자 함수 실행 완료");
    }

    private void Start()
    {
        // 테스트 용도
        StartCoroutine(UpdateTime());
    }

    // 리얼타임(float) -> 00:00 AM/PM 형식(string) 변환
    string GetGameTimeStringAMPM(float realTimeSeconds)
    {
        int totalMinutes = Mathf.FloorToInt(realTimeSeconds); // 소수점 버림

        int startHour = 9;
        int currentHour24 = (startHour + totalMinutes / 60) % 24;
        int currentMinute = totalMinutes % 60;

        int hour12 = currentHour24 % 12;
        if (hour12 == 0) hour12 = 12;

        string ampm = currentHour24 < 12 ? "AM" : "PM";

        return $"{currentTimeData.CurrentDay.Value + 1}일차 {ampm} {hour12:D2}:{currentMinute:D2}";
    }

    IEnumerator UpdateTime()
    {
        UIManager um = UIManager.Instance;
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

            timeToString = GetGameTimeStringAMPM(currentTimeData.CurrentTime);
            um.hudGroup.HUD_Time.tmp_Time.text = timeToString;
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

    public TimeZoneSetting GetTimeZoneSetting()
    {
        return timeZoneSetting;
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
    public ObservableProperty<int> CurrentDay = new ObservableProperty<int>();
    public ObservableProperty<TimeZoneState> TZ_State = new ObservableProperty<TimeZoneState>();
}