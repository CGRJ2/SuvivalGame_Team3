using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    // ������ �ٸ� �Ŵ����麸�� �� �������� �ʱ�ȭ �ؾ� ��
    PlayerManager pm;
    public Temp_BaseCampData baseCampData;

    public int MaxLevel { get; private set; }
    public BaseCampLevelUpCondition[] levelUpConditions;

    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
        baseCampData = new Temp_BaseCampData();
    }

    private void Start()
    {
        InitLevelUpConditions();
    }

    // Recoureces���� ���̽�ķ�� ������ ����(��ũ���ͺ�obj) �迭�� �޾ƿ� ���� ������ �����ϱ�
    private void InitLevelUpConditions()
    {
        levelUpConditions = Resources.LoadAll<BaseCampLevelUpCondition>("BaseCampLevelUpConditions");
        Array.Sort(levelUpConditions, (a, b) => a.currentLevel.CompareTo(b.currentLevel));
    }

}
