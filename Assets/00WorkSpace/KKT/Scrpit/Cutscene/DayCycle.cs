using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light directionalLight; // Inspector에서 할당
    public Gradient lightColor; // 빛의 색상 변화
    public AnimationCurve lightIntensity; // 빛의 밝기 변화 (0~1)


    [SerializeField] private float dayDuration; // 하루가 몇 초에 끝나는지 (예시: 60초)

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

        // 태양 각도 변화 (0~360도)
        float sunAngle = normalizedTime * 360f + 15f; // -90 시작점이 동쪽
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // 색상과 밝기 변화
        //directionalLight.color = lightColor.Evaluate(normalizedTime);
        //directionalLight.intensity = lightIntensity.Evaluate(normalizedTime);
    }
}
