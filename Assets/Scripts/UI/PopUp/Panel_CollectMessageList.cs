using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_CollectMessageList : MonoBehaviour
{
    public List<CollectMessageSlot> collectMessageSlots;


    // 우선 한개만 쓰고 시간 남으면 여러개 슥슥 올라가는 방식 추가

    public void Awake()
    {
        //StartCoroutine(ShowAndHide(collectMessageSlots[0].gameObject, collectMessageSlots[0].tmp_CollectMessage, msg, duration));

    }
    
}
