using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIGroup : MonoBehaviour
{
    public GameObject basePanel;
    public GameObject panel_MaxUpgraded;
    public Panel_Upgrading panel_Upgrading;
    public UpgradeRequires upgradeRequires;
    public Button Btn_Upgrade;


    bool isUpgrading = false;
    float upgradeProgressingTime;

    Coroutine upgradingCoroutine;


    private void Awake() => Init();


    private void OnDisable()
    {
        // [임시] 일단 파괴될 때로 테스트 해보고, 게임 종료 or 타이틀 씬 이동 시 호출
        SaveUpgradeProgress();
    }
    public void Init()
    {
        UIManager.Instance.upgradeGroup = this;
        upgradeRequires.Init();
    }

    // 업그레이드 진행상황 => BaseCampData에 저장
    public void SaveUpgradeProgress()
    {
        // 진행중인 작업이 있다면
        if (upgradingCoroutine != null)
        {
            StopCoroutine(upgradingCoroutine);
            BaseCampManager.Instance.baseCampData.upgradingProcess.isUpgrading = true;
            BaseCampManager.Instance.baseCampData.upgradingProcess.proceededTime = upgradeProgressingTime;
        }
    }

    // 저장된 게임 불러올 시 호출
    // BaseCampData에 업그레이드 진행상황 데이터가 남아있다면 => 업그레이드 이어서 진행 
    public void SyncUpgradeProgressFromLoadData(BaseCampData baseCampData)
    {
        // 동기화 후 데이터 내부의 진행상황 삭제
        if (baseCampData.upgradingProcess.isUpgrading)
        {
            upgradeProgressingTime = baseCampData.upgradingProcess.proceededTime;
            upgradingCoroutine = StartCoroutine(UpgradingCoroutine());

            // 데이터 초기화
            baseCampData.upgradingProcess = new UpgradingProcess();
        }
    }


    public void OpenPanel_Base()
    {
        BaseCampManager bcm = BaseCampManager.Instance;
        UIManager.Instance.OpenPanel(basePanel);
        
        // 업그레이드 도중이라면
        if (isUpgrading)
        {
            panel_Upgrading.gameObject.SetActive(true);
            return;
        }
        else
        {
            if(!panel_Upgrading.gameObject.activeSelf)
                panel_Upgrading.gameObject.SetActive(false);
        }

        // 업그레이드 가능 상태일 때 (최대 레벨이 아닐 때) 조건들 업데이트
        if (bcm.baseCampData.CurrentCampLevel.Value < bcm.MaxLevel)
        {
            UpdateUpgradeRequiresPanelState();
        }
        // 이미 최대 레벨이라면
        else
        {
            // 최고 등급 패널 활성화
            if (!panel_MaxUpgraded.activeSelf)
                panel_MaxUpgraded.SetActive(true);
        }
    }

    public void UpdateUpgradeRequiresPanelState()
    {
        bool isCheckedIngrediantsRequires = upgradeRequires.IsRequiresSufficent();
        bool isCheckedStageRequired = upgradeRequires.IsStageConditionUnlocked();

        // 재료 & 스테이지 해금 => 두 조건 모두 만족 시 업그레이드 버튼 활성화
        if (isCheckedIngrediantsRequires && isCheckedStageRequired)
        {
            Btn_Upgrade.interactable = true;
        }
        else
        {
            Btn_Upgrade.interactable = false;
        }
    }


    // 업그레이트 버튼 이벤트에서 호출
    public void StartUpgrade()
    {
        upgradingCoroutine = StartCoroutine(UpgradingCoroutine());
    }

    IEnumerator UpgradingCoroutine()
    {
        isUpgrading = true;
        panel_Upgrading.gameObject.SetActive(true);

        panel_Upgrading.UpdateView(upgradeRequires.currentUpgradeCondition.upgradingTime);

        while (upgradeProgressingTime < upgradeRequires.currentUpgradeCondition.upgradingTime)
        {
            yield return new WaitForSeconds(1f);
            upgradeProgressingTime += 1;

            // 남은 시간 view 업데이트
            float remainTime = upgradeRequires.currentUpgradeCondition.upgradingTime - upgradeProgressingTime;
            panel_Upgrading.UpdateView(remainTime);
        }

        // 업그레이드 완료
        UpgradeComplete();
    }

    private void UpgradeComplete()
    {
        isUpgrading = false;
        upgradeProgressingTime = 0;

        // 레벨업 하고
        BaseCampManager.Instance.LevelUp();

        // 조건 패널에 레벨업 된 상태를 반영
        UpdateUpgradeRequiresPanelState();

        // 업데이트 완료 후 업그레이딩 패널 꺼주기
        panel_Upgrading.gameObject.SetActive(false);
    }
}
