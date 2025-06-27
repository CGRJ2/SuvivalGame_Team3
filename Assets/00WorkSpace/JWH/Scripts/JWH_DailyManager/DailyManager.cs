using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DailyState
//{
//      Morning   // 03:00 ~ 12:00
//      Afternoon // 12:00 ~ 15:00
//      Night     // 15:00 ~ 21:00
//      Dawn      // 21:00 ~ 03:00 (다음 날)
//}

public class DailyManager : MonoBehaviour
{
    public static DailyManager Instance { get; private set; }//싱글톤 인스턴스
    public TimeSpan CurrentTime { get; private set; }//인게임시간
    public int CurrentDay { get; private set; } = 1;//인게임날짜
    //public DailyState CurrentState { get; private set; }//현재 인게임시간
    private int lastLoggedHour = -1;
    //public bool isPaused;// 일시정지

    [SerializeField] private float realtime = 900f; // 인게임 15분=900초
    [SerializeField] private int maxDays = 14;
    public event Action<TimeSpan> OnTimeUpdated;// 외부용 이벤트
    public event Action<int> OnDayChanged;// 외부용 이벤트
    //public ObservableProperty<TimeSpan> CurrentOnTimeProperty = new ObservableProperty<int>();
    //public ObservableProperty<int> CurrentOnDayProperty = new ObservableProperty<int>();

    private float timecal;//누적시간관리아무튼중요함

    private void Awake()//싱글
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        CurrentTime = new TimeSpan(3, 0, 0); // 시작 시간
        CurrentDay = 1;
        //CurrentTime = CurrentTime.Add(TimeSpan.FromMinutes(1));
        //CurrentOnTimeProperty.Value = CurrentTime;
        //UpdateDailyState(); // 초기 상태
    }

    private void Update()
    {
        float secondsPerMinute = realtime / 1440f;//현실 시간 건들면 안됨
        timecal += Time.deltaTime;

        if (timecal >= secondsPerMinute)
        {
            CurrentTime = CurrentTime.Add(TimeSpan.FromMinutes(1));
            timecal = 0f;

            if (CurrentTime.TotalMinutes >= 1440)
            {
                CurrentTime = TimeSpan.Zero;
                ToNextDay();
            }

            //UpdateDailyState();
            OnTimeUpdated?.Invoke(CurrentTime);
            PrintDebugLog();
        }
    }


    private void ToNextDay()
    {
        if (CurrentDay < maxDays)
        {
            CurrentDay++;
            OnDayChanged?.Invoke(CurrentDay);
            //CurrentDay++;
            //CurrentOnDayProperty.Value = CurrentDay;
        }
        else
        {
            Debug.Log("마지막날");
        }
    }

    private void PrintDebugLog()
    {
        int hour = CurrentTime.Hours;
        int minute = CurrentTime.Minutes;

        // 1시간마다 로그
        if (hour != lastLoggedHour)
        {
            lastLoggedHour = hour;
            Debug.Log($"인게임 시간 {CurrentTime:hh\\:mm} / {CurrentDay}일차 / 상태: ");
        }
    }


    //public void PauseTime()
    //{
    //    isPaused = true;
    //}

    //public void ResumeTime()
    //{
    //    isPaused = false;
    //}

    //private void UpdateDailyState()
    //{
    //    int hour = CurrentTime.Hours;
    //    if (hour >= 3 && hour < 12)
    //        CurrentState = DailyState.Morning;//오전
    //    else if (hour >= 12 && hour < 15)
    //        CurrentState = DailyState.Afternoon;//오후
    //    else if (hour >= 15 && hour < 21)
    //        CurrentState = DailyState.Dawn;//밤
    //       else // 21:00 ~ 03:00
    //      CurrentState = DailyState.Dawn;//새벽
    //}


}