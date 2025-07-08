using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light directionalLight; // Inspector���� �Ҵ�
    public Gradient lightColor; // ���� ���� ��ȭ
    public AnimationCurve lightIntensity; // ���� ��� ��ȭ (0~1)


    [SerializeField] private float dayDuration; // �Ϸ簡 �� �ʿ� �������� (����: 60��)

    [SerializeField] private float time;


    private void Start()
    {
        DailyManager dailyManager = DailyManager.Instance;
        dayDuration = dailyManager.GetTimeZoneSetting().DayCycleTime;
        time = dailyManager.currentTimeData.CurrentTime;
    }

    void Update()
    {
        time += Time.deltaTime;
        float normalizedTime = (time % dayDuration) / dayDuration;

        // �¾� ���� ��ȭ (0~360��)
        float sunAngle = normalizedTime * 360f + 15f; // -90 �������� ����
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // ����� ��� ��ȭ
        //directionalLight.color = lightColor.Evaluate(normalizedTime);
        //directionalLight.intensity = lightIntensity.Evaluate(normalizedTime);
    }
}
