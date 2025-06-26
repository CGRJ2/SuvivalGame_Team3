using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase 
{

    static PlayerManager pm = PlayerManager.Instance;
    // ���� �Ŵ��� => ���� ����
    // ����Ŀ �Ŵ��� => ����Ŀ ���� 
    // ������ �Ŵ��� => ���� ���
    // ��� �Ŵ��� => ��� ����, �Ǻ� ���

    public static Dictionary<string, Action> ConsumeEffectDic = new Dictionary<string, Action>()
    {
        { "���� ���͸�", () => PlayerManager.Instance.instancePlayer.Status.Battery.Value += 10},
        { "���A", () => Debug.Log("��� �����ߴ�")},
        { "TestItem", () => Debug.Log("���׸� �������.")},
        { "�ƹ�ư����", () => pm.instancePlayer.Status.WillPowerAdjust(10)},
        { "�ƹ�ư ����", () => pm.instancePlayer.Status.WillPowerAdjust(-10)},
        { "�ƹ�ư Ĺ��", () => Debug.Log("�տ� ����")},
        { "�Һ�B", () => Debug.Log("B �ȳ�")},
    };

    public static Dictionary<string, Action> EquipEffectDic = new Dictionary<string, Action>()
    {
        { "�ƹ�ư Ĺ��", () => Debug.Log("��ô���� �غ����")}, // ���⼭ ������ �տ� ���� �� ���� Ű ���� �� ȿ�� ����
    };
    }
