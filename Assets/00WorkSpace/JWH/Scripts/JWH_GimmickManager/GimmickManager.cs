using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DailyState
{
    Morning,   // 03:00 ~ 12:00
    Afternoon, // 12:00 ~ 15:00
    Night,     // 15:00 ~ 21:00
    Dawn       // 21:00 ~ 03:00 (다음날)
}

public class GimmickManager : MonoBehaviour
{
        public static GimmickManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        public DailyState CurrentState
        {
            get
            {
                TimeSpan time = DailyManager.Instance.CurrentTime;
                int hour = time.Hours;

                if (hour >= 3 && hour < 12)
                    return DailyState.Morning;
                else if (hour >= 12 && hour < 15)
                    return DailyState.Afternoon;
                else if (hour >= 15 && hour < 21)
                    return DailyState.Night;
                else 
                    return DailyState.Dawn;
            }
        }

    public bool CanSaveNow() //테스트
    {
        return CurrentState == DailyState.Night || CurrentState == DailyState.Dawn;
    }
}

