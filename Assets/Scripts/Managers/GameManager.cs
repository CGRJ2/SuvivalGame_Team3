using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] SuvivalSystemManager suvivalSystemManager;
    [SerializeField] DailyManager dailyManager;
    [SerializeField] DataManager dataManager;
    [SerializeField] BaseCampManager baseCampManager;
    private void Awake() => Init();



    private void Update() => UpdateByOreder();

        private void Init()
    {
        base.SingletonInit();
        InitalizeOrderSetting();
    }

    // 게임매니저 제외한 싱글톤 객체들 초기화 순서 세팅
    private void InitalizeOrderSetting()
    {
        // 초기화할 순서대로 나열
        dataManager.Init();
        uiManager.Init();
        playerManager.Init();
        suvivalSystemManager.Init();
        dailyManager.Init();
        baseCampManager.Init();
    }

    public void UpdateByOreder()
    {

    }
}
