using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_Upgrading : MonoBehaviour
{
    public TMP_Text tmp_ProgressTime;

    public void UpdateView(float remainTime)
    {
        int minutes = Mathf.FloorToInt(remainTime / 60f);
        int seconds = Mathf.FloorToInt(remainTime % 60f);

        tmp_ProgressTime.text = string.Format("남은 시간: {0:00}:{1:00}", minutes, seconds);
    }
}
