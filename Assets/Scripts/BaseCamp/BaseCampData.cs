using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BaseCampData
{
    // 현재 레벨 정보
    public ObservableProperty<int> CurrentCampLevel = new ObservableProperty<int>();

    // 진행중인 업그레이드 정보
    public UpgradingProcess upgradingProcess = new UpgradingProcess();

    // 
    public Vector3 baseCampPosition;
    public Quaternion baseCampRotation;
}

[System.Serializable]
public class UpgradingProcess
{
    public bool isUpgrading = false;
    public float proceededTime = 0;
}

