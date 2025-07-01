using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLight : MonoBehaviour
{
    private bool isLighted = false; // �������� Ȱ��ȭ ���¸� ��Ÿ���� ����
    public float intensity;


    void Update()
    {
        ToggleFlashlight();     // ������ Ȱ��ȭ �Է��� ����
    }

    public void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isLighted = !isLighted;

            if (isLighted)
                GetComponent<Light>().intensity = intensity;
            else
                GetComponent<Light>().intensity = 0;
        }
    }
}
