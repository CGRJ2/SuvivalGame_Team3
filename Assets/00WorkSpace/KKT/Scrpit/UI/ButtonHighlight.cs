using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    public Button[] tabButtons; // Inspector�� ��� �� ��ư ����
    public Outline[] outlines;  // �� ��ư�� ���� Outline ����

    // ���õ� �ε���(0=����, 1=������, 2=���� ...)
    public void SelectTab(int index)
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            // Ȱ��ȭ�� �Ǹ� Outline On, �������� Off
            if (outlines[i] != null)
                outlines[i].enabled = (i == index);
        }
    }
}
