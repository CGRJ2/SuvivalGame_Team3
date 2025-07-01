using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// [���̺�/�ε� ������]
[System.Serializable]
public class BaseCampData
{
    public ObservableProperty<int> CurrentCampLevel = new ObservableProperty<int>();

    public UpgradingProcess upgradingProcess = new UpgradingProcess();
}

[System.Serializable]
public class UpgradingProcess
{
    public bool isUpgrading = false;
    public float proceededTime = 0;
}

