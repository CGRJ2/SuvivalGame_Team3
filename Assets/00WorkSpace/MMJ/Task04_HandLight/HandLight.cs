using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLight : MonoBehaviour
{
    private bool isLighted = false; // 손전등의 활성화 상태를 나타내는 변수
    public float intensity;


    void Update()
    {
        ToggleFlashlight();     // 손전등 활성화 입력을 감지
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
