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



    // ������ �Ҹ� ȿ�� ������ ���̽� => Action<�Ű�����, �Ű�����>
    public static Dictionary<string, Action> ConsumeEffectDic = new Dictionary<string, Action>()
    {

    };
    

    // �Ϲ� ��� (����Ʈ ������, ���� ��)
    public static Dictionary<string, Action> AttackOnHandEffectDic = new Dictionary<string, Action>()
    {
        // Consumable > Generic
        
        { "Ĺ��", () => Debug.Log("A���� ���� ����Ѵ�")},
        { "�Ϲ� �Һ� �׽�Ʈ ������", () => Debug.Log("�Ϲ� �Һ� �׽�Ʈ ���� ���")},


        // Equipment > Generic(Tool)
        { "������", () => Debug.Log("���� �Ҵ�")}, // ���⼭ ������ �տ� ���� �� ���� Ű ���� �� ȿ�� ����

        // Quest > Generic()
        { "����A", () => Debug.Log("A���� ���� ����Ѵ�")},



    };


   
   


}
