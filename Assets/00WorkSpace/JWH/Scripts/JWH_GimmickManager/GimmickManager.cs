using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DailyState
{
    Morning,   // 03:00 ~ 12:00
    Afternoon, // 12:00 ~ 15:00
    Night,     // 15:00 ~ 21:00
    Dawn       // 21:00 ~ 03:00 (������)
}



        public static class GimmickManager
        { 

        public static DailyState CurrentState
        {
            get
            {
                TimeSpan time = DailyManager.Instance.CurrentTime;
                int hour = time.Hours;

                if (hour >= 3 && hour < 12) return DailyState.Morning;
                else if (hour >= 12 && hour < 15) return DailyState.Afternoon;
                else if (hour >= 15 && hour < 21) return DailyState.Night;
                else return DailyState.Dawn;
            }
        }

        public static bool CanSaveNow() //�׽�Ʈ
        {
        return CurrentState == DailyState.Night || CurrentState == DailyState.Dawn;
        }
}

