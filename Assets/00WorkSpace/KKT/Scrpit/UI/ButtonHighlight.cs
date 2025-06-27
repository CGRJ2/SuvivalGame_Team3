using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    public Button[] tabButtons; // Inspector에 모든 탭 버튼 연결
    public Outline[] outlines;  // 각 버튼에 붙은 Outline 연결

    // 선택된 인덱스(0=지도, 1=아이템, 2=설정 ...)
    public void SelectTab(int index)
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            // 활성화된 탭만 Outline On, 나머지는 Off
            if (outlines[i] != null)
                outlines[i].enabled = (i == index);
        }
    }
}
