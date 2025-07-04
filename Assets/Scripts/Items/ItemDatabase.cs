using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase 
{
    // ���� �Ŵ��� => ���� ����
    // ����Ŀ �Ŵ��� => ����Ŀ ���� 
    // ������ �Ŵ��� => ���� ���
    // ��� �Ŵ��� => ��� ����, �Ǻ� ���



    // ������ �Ҹ� ȿ�� ������ ���̽� => Action<�Ű�����, �Ű�����>
    public static Dictionary<string, Action> ConsumeEffectDic = new Dictionary<string, Action>()
    {
        { "��ȸ�� ķ��", () => BaseCampManager.Instance.UseTempCampItem()},
    };
    

    // �Ϲ� ��� (����Ʈ ������, ���� ��)
    public static Dictionary<string, Action> AttackOnHandEffectDic = new Dictionary<string, Action>()
    {
        { "Ĺ��", () => Debug.Log("�տ� �� Ĺ���� ������")},

        // Equipment > Generic(Tool)
        { "������", () => Debug.Log("���� �Ҵ�")}, // ���⼭ ������ �տ� ���� �� ���� Ű ���� �� ȿ�� ����

        // Quest > Generic()
        { "����A", () => Debug.Log("A���� ���� ����Ѵ�")},
    };


   
   


}
