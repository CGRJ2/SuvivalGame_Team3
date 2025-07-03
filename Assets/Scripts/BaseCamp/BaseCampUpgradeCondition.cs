using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BaseCampUpgradeCondition", menuName = "New BaseCampUpgradeCondition")]

public class BaseCampUpgradeCondition : ScriptableObject
{
    [Header("���� ���� ����(���׷��̵� ������ ���� �ƴ�!)")]
    public int currentLevel;

    [Header("�ش� ���׷��̵� �� ���̽� ķ���� ��ȭ�� ���")]
    public Sprite baseCampSprite;

    [Header("���׷��̵� ���� �ð�")]
    public float upgradingTime;

    [Header("����: �������� �ر� ����")]
    public StageData needUnlockStage;

    [Header("����: ������ ���� ����")]
    public List<ItemRequirement> requiredItems;

}
