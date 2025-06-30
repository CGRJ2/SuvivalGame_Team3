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

    // ���ӸŴ��� ������ �̱��� ��ü�� �ʱ�ȭ ���� ����
    private void InitalizeOrderSetting()
    {
        // �ʱ�ȭ�� ������� ����
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
