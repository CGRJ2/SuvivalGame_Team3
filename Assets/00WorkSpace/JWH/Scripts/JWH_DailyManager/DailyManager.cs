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
        public static DailyManager Instance { get; private set; }//�̱��� �ν��Ͻ�
        public TimeSpan CurrentTime { get; private set; }//�ΰ��ӽð�
        public int CurrentDay { get; private set; } = 1;//�ΰ��ӳ�¥
        //public DailyState CurrentState { get; private set; }//���� �ΰ��ӽð�
        private int lastLoggedHour = -1;

        [SerializeField] private float realtime = 900f; // �ΰ��� 15��=900��
        [SerializeField] private int maxDays = 14;
        public event Action<TimeSpan> OnTimeUpdated;// �ܺο� �̺�Ʈ
        public event Action<int> OnDayChanged;// �ܺο� �̺�Ʈ
        
        private float timecal;//�����ð������ƹ�ư�߿���

        private void Awake()//�̱�
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            CurrentTime = new TimeSpan(9, 0, 0); // ���� �ð�
            CurrentDay = 1;
            //UpdateDailyState(); // �ʱ� ����
        }

        private void Update()
        {
            float secondsPerMinute = realtime / 1440f;//���� �ð� �ǵ�� �ȵ�
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
            Debug.Log("��������");
        }
    }

    private void PrintDebugLog()
    {
        int hour = CurrentTime.Hours;
        int minute = CurrentTime.Minutes;

        // 1�ð����� �α�
        if (hour != lastLoggedHour)
        {
            lastLoggedHour = hour;
            Debug.Log($"�ΰ��� �ð� {CurrentTime:hh\\:mm} / {CurrentDay}����");
        }
    }

        //private void UpdateDailyState()
        //{
        //    int hour = CurrentTime.Hours;
        //    if (hour >= 9 && hour < 15)
        //        CurrentState = DailyState.Morning;//��ħ
        //    else if (hour >= 15 && hour < 22)
        //        CurrentState = DailyState.Afternoon;//��
        //    else
        //        CurrentState = DailyState.Dawn;//����
        //}


    }