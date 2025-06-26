using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DailyState
//{
//      Morning   // 03:00 ~ 12:00
//      Afternoon // 12:00 ~ 15:00
//      Night     // 15:00 ~ 21:00
//      Dawn      // 21:00 ~ 03:00 (���� ��)
//}

public class DailyManager : MonoBehaviour
{
    public static DailyManager Instance { get; private set; }//�̱��� �ν��Ͻ�
    public TimeSpan CurrentTime { get; private set; }//�ΰ��ӽð�
    public int CurrentDay { get; private set; } = 1;//�ΰ��ӳ�¥
    //public DailyState CurrentState { get; private set; }//���� �ΰ��ӽð�
    private int lastLoggedHour = -1;
    //public bool isPaused;// �Ͻ�����

    [SerializeField] private float realtime = 900f; // �ΰ��� 15��=900��
    [SerializeField] private int maxDays = 14;
    public event Action<TimeSpan> OnTimeUpdated;// �ܺο� �̺�Ʈ
    public event Action<int> OnDayChanged;// �ܺο� �̺�Ʈ
    //public ObservableProperty<TimeSpan> CurrentOnTimeProperty = new ObservableProperty<int>();
    //public ObservableProperty<int> CurrentOnDayProperty = new ObservableProperty<int>();

    private float timecal;//�����ð������ƹ�ư�߿���

    private void Awake()//�̱�
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        CurrentTime = new TimeSpan(3, 0, 0); // ���� �ð�
        CurrentDay = 1;
        //CurrentTime = CurrentTime.Add(TimeSpan.FromMinutes(1));
        //CurrentOnTimeProperty.Value = CurrentTime;
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
            //CurrentDay++;
            //CurrentOnDayProperty.Value = CurrentDay;
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
            Debug.Log($"�ΰ��� �ð� {CurrentTime:hh\\:mm} / {CurrentDay}���� / ����: ");
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
    //        CurrentState = DailyState.Morning;//����
    //    else if (hour >= 12 && hour < 15)
    //        CurrentState = DailyState.Afternoon;//����
    //    else if (hour >= 15 && hour < 21)
    //        CurrentState = DailyState.Dawn;//��
    //       else // 21:00 ~ 03:00
    //      CurrentState = DailyState.Dawn;//����
    //}


}