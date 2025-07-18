using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase 
{
    // 몬스터 매니저 => 몬스터 판정
    // 스토커 매니저 => 스토커 판정 
    // 데이터 매니저 => 저장 기능
    // 기믹 매니저 => 기믹 해제, 판별 기능



    // 아이템 소모 효과 데이터 베이스 => Action<매개변수, 매개변수>
    public static Dictionary<string, Action> ConsumeEffectDic = new Dictionary<string, Action>()
    {
        { "일회용 캠프", () => BaseCampManager.Instance.UseTempCampItem()},
    };
    

    // 일반 사용 (퀘스트 아이템, 지도 등)
    public static Dictionary<string, Action> AttackOnHandEffectDic = new Dictionary<string, Action>()
    {
        { "캣닢", () => Debug.Log("손에 든 캣닢을 던진다")},

        // Equipment > Generic(Tool)
        { "손전등", () => Debug.Log("불을 켠다")}, // 여기서 아이템 손에 장착 후 공격 키 누를 때 효과 정리

        // Quest > Generic()
        { "열쇠A", () => Debug.Log("A방의 문을 언락한다")},
    };


   
   


}
