using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light directionalLight; // Inspector���� �Ҵ�
    public float dayDuration = 60f; // �Ϸ簡 �� �ʿ� �������� (����: 60��)
    public Gradient lightColor; // ���� ���� ��ȭ
    public AnimationCurve lightIntensity; // ���� ��� ��ȭ (0~1)

    private float time;

    void Update()
    {
        time += Time.deltaTime;
        float normalizedTime = (time % dayDuration) / dayDuration;

        // �¾� ���� ��ȭ (0~360��)
        float sunAngle = normalizedTime * 360f - 90f; // -90 �������� ����
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // ����� ��� ��ȭ
        directionalLight.color = lightColor.Evaluate(normalizedTime);
        directionalLight.intensity = lightIntensity.Evaluate(normalizedTime);
    }
}
