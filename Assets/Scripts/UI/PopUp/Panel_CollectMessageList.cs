using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_CollectMessageList : MonoBehaviour
{
    public List<CollectMessageSlot> collectMessageSlots;


    // �켱 �Ѱ��� ���� �ð� ������ ������ ���� �ö󰡴� ��� �߰�

    public void Awake()
    {
        //StartCoroutine(ShowAndHide(collectMessageSlots[0].gameObject, collectMessageSlots[0].tmp_CollectMessage, msg, duration));

    }
    
}
