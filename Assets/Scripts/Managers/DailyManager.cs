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

    [Header("�ð� ������ ��ȯ")]
    [SerializeField] string timeToString;




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
        currentTimeData.CurrentDay.Value = saveDataGroup.currentTimeData.CurrentDay.Value;
        currentTimeData.CurrentTime = saveDataGroup.currentTimeData.CurrentTime;
        currentTimeData.TZ_State.Value = saveDataGroup.currentTimeData.TZ_State.Value;


        Debug.Log("���ϸ� �Ŵ��� ������ �Լ� ���� �Ϸ�");
    }

    private void Start()
    {
        // �׽�Ʈ �뵵
        StartCoroutine(UpdateTime());
    }

    // ����Ÿ��(float) -> 00:00 AM/PM ����(string) ��ȯ
    string GetGameTimeStringAMPM(float realTimeSeconds)
    {
        int totalMinutes = Mathf.FloorToInt(realTimeSeconds); // �Ҽ��� ����

        int startHour = 9;
        int currentHour24 = (startHour + totalMinutes / 60) % 24;
        int currentMinute = totalMinutes % 60;

        int hour12 = currentHour24 % 12;
        if (hour12 == 0) hour12 = 12;

        string ampm = currentHour24 < 12 ? "AM" : "PM";

        return $"{currentTimeData.CurrentDay.Value + 1}���� {ampm} {hour12:D2}:{currentMinute:D2}";
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
            Debug.Log("�������� ���� => ���� ����");
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