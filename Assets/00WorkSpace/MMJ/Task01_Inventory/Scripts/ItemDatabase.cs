using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase 
{

    static PlayerManager pm = PlayerManager.Instance;
    // 몬스터 매니저 => 몬스터 판정
    // 스토커 매니저 => 스토커 판정 
    // 데이터 매니저 => 저장 기능
    // 기믹 매니저 => 기믹 해제, 판별 기능

    public static Dictionary<string, Action> ConsumeEffectDic = new Dictionary<string, Action>()
    {
        { "보조 배터리", () => PlayerManager.Instance.instancePlayer.Status.Battery.Value += 10},
        { "장비A", () => Debug.Log("장비를 장착했다")},
        { "TestItem", () => Debug.Log("버그를 막아줬다.")},
        { "아무튼포션", () => pm.instancePlayer.Status.WillPowerAdjust(10)},
        { "아무튼 독약", () => pm.instancePlayer.Status.WillPowerAdjust(-10)},
        { "아무튼 캣잎", () => Debug.Log("손에 장착")},
        { "소비B", () => Debug.Log("B 냠냠")},
    };

    public static Dictionary<string, Action> EquipEffectDic = new Dictionary<string, Action>()
    {
        { "아무튼 캣잎", () => Debug.Log("투척무기 준비상태")}, // 여기서 아이템 손에 장착 후 공격 키 누를 때 효과 정리
    };
    }
