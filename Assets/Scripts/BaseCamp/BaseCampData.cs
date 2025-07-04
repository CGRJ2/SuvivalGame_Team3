using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BaseCampData
{
    // ���� ���� ����
    public ObservableProperty<int> CurrentCampLevel = new ObservableProperty<int>();

    // �������� ���׷��̵� ����
    public UpgradingProcess upgradingProcess = new UpgradingProcess();

    // 
    public Transform baseCampTransform;
}

[System.Serializable]
public class UpgradingProcess
{
    public bool isUpgrading = false;
    public float proceededTime = 0;
}

