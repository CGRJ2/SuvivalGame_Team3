using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] UIManager uiManager;

    // 게임 상태 : 장사 대기 > 장사 시작 > 장사 종료 > 정산 > 장사 대기 ... ////// +일시정지
    private void Awake() => Init();

    private void Init()
    {
        base.SingletonInit();
        InitalizeOrderSetting();
    }

    // 게임매니저 제외한 싱글톤 객체들 초기화 순서 세팅
    private void InitalizeOrderSetting()
    {
        // 초기화할 순서대로 나열
        uiManager.Init();
    }

}
