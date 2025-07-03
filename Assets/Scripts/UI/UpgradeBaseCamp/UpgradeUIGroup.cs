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
        // [�ӽ�] �ϴ� �ı��� ���� �׽�Ʈ �غ���, ���� ���� or Ÿ��Ʋ �� �̵� �� ȣ��
        SaveUpgradeProgress();
    }
    public void Init()
    {
        UIManager.Instance.upgradeGroup = this;
        upgradeRequires.Init();
    }

    // ���׷��̵� �����Ȳ => BaseCampData�� ����
    public void SaveUpgradeProgress()
    {
        // �������� �۾��� �ִٸ�
        if (upgradingCoroutine != null)
        {
            StopCoroutine(upgradingCoroutine);
            BaseCampManager.Instance.baseCampData.upgradingProcess.isUpgrading = true;
            BaseCampManager.Instance.baseCampData.upgradingProcess.proceededTime = upgradeProgressingTime;
        }
    }

    // ����� ���� �ҷ��� �� ȣ��
    // BaseCampData�� ���׷��̵� �����Ȳ �����Ͱ� �����ִٸ� => ���׷��̵� �̾ ���� 
    public void SyncUpgradeProgressFromLoadData(BaseCampData baseCampData)
    {
        // ����ȭ �� ������ ������ �����Ȳ ����
        if (baseCampData.upgradingProcess.isUpgrading)
        {
            upgradeProgressingTime = baseCampData.upgradingProcess.proceededTime;
            upgradingCoroutine = StartCoroutine(UpgradingCoroutine());

            // ������ �ʱ�ȭ
            baseCampData.upgradingProcess = new UpgradingProcess();
        }
    }


    public void OpenPanel_Base()
    {
        BaseCampManager bcm = BaseCampManager.Instance;
        UIManager.Instance.OpenPanel(basePanel);
        
        // ���׷��̵� �����̶��
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

        // ���׷��̵� ���� ������ �� (�ִ� ������ �ƴ� ��) ���ǵ� ������Ʈ
        if (bcm.baseCampData.CurrentCampLevel.Value < bcm.MaxLevel)
        {
            UpdateUpgradeRequiresPanelState();
        }
        // �̹� �ִ� �����̶��
        else
        {
            // �ְ� ��� �г� Ȱ��ȭ
            if (!panel_MaxUpgraded.activeSelf)
                panel_MaxUpgraded.SetActive(true);
        }
    }

    public void UpdateUpgradeRequiresPanelState()
    {
        bool isCheckedIngrediantsRequires = upgradeRequires.IsRequiresSufficent();
        bool isCheckedStageRequired = upgradeRequires.IsStageConditionUnlocked();

        // ��� & �������� �ر� => �� ���� ��� ���� �� ���׷��̵� ��ư Ȱ��ȭ
        if (isCheckedIngrediantsRequires && isCheckedStageRequired)
        {
            Btn_Upgrade.interactable = true;
        }
        else
        {
            Btn_Upgrade.interactable = false;
        }
    }


    // ���׷���Ʈ ��ư �̺�Ʈ���� ȣ��
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

            // ���� �ð� view ������Ʈ
            float remainTime = upgradeRequires.currentUpgradeCondition.upgradingTime - upgradeProgressingTime;
            panel_Upgrading.UpdateView(remainTime);
        }

        // ���׷��̵� �Ϸ�
        UpgradeComplete();
    }

    private void UpgradeComplete()
    {
        isUpgrading = false;
        upgradeProgressingTime = 0;

        // ������ �ϰ�
        BaseCampManager.Instance.LevelUp();

        // ���� �гο� ������ �� ���¸� �ݿ�
        UpdateUpgradeRequiresPanelState();

        // ������Ʈ �Ϸ� �� ���׷��̵� �г� ���ֱ�
        panel_Upgrading.gameObject.SetActive(false);
    }
}
