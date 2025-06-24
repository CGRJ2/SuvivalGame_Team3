using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //public enum DailyState
    //{
    //    Morning,   // 09:00 ~ 15:00
    //    Afternoon, // 15:00 ~ 22:00
    //    Dawn       // 22:00 ~ 09:00
    //}

    public class DailyManager : MonoBehaviour
    {
        public static DailyManager Instance { get; private set; }//싱글톤 인스턴스
        public TimeSpan CurrentTime { get; private set; }//인게임시간
        public int CurrentDay { get; private set; } = 1;//인게임날짜
        //public DailyState CurrentState { get; private set; }//현재 인게임시간
        private int lastLoggedHour = -1;

        [SerializeField] private float realtime = 900f; // 인게임 15분=900초
        [SerializeField] private int maxDays = 14;
        public event Action<TimeSpan> OnTimeUpdated;// 외부용 이벤트
        public event Action<int> OnDayChanged;// 외부용 이벤트
        
        private float timecal;//누적시간관리아무튼중요함

        private void Awake()//싱글
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            CurrentTime = new TimeSpan(9, 0, 0); // 시작 시간
            CurrentDay = 1;
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
            Debug.Log($"인게임 시간 {CurrentTime:hh\\:mm} / {CurrentDay}일차");
        }
    }

        //private void UpdateDailyState()
        //{
        //    int hour = CurrentTime.Hours;
        //    if (hour >= 9 && hour < 15)
        //        CurrentState = DailyState.Morning;//아침
        //    else if (hour >= 15 && hour < 22)
        //        CurrentState = DailyState.Afternoon;//낮
        //    else
        //        CurrentState = DailyState.Dawn;//새벽
        //}


    }