using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_SuvivalState : MonoBehaviour
{
    public float initMax;

    [Header("현재 수치(TMP)")]
    public TMP_Text tmp_Current;
    public Image HUD_ChainedCur;

    [Header("최대 수치(TMP)")]
    public TMP_Text tmp_Max;
    public Image HUD_ChainedMax;


    public void UpdateStateNumb_View(float now)
    {
        // 현재 숫자 업데이트
        tmp_Current.text = now.ToString();

        // HUD 바 업데이트
        HUD_ChainedCur.fillAmount = Mathf.Clamp01(now / initMax);
    }

    public void UpdateMaxStateNumb_View(float now)
    {
        tmp_Max.text = now.ToString();

        if (HUD_ChainedMax != null)
            HUD_ChainedMax.fillAmount = Mathf.Clamp01(now / initMax);
    }
}
