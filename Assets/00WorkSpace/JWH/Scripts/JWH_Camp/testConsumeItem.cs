using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
//public struct ResourceCost
//{
//    public string resourceName; // �ڿ��� �̸�
//    public int quantity;   // �ʿ��� ����
//}

//public class ConsumeItem : MonoBehaviour
//{
//    private Dictionary<string, int> playerResources = new Dictionary<string, int>(); //�κ��丮�� ������ ����

//    public KeyCode addResourceKey = KeyCode.R;//�׽�Ʈ�ڿ�
//    public int resourceAddAmount = 10;

//    void Start()
//    {
//        // �÷��̾ ������ �ڿ�
//        playerResources.Add("CampResource", 0);
//        LogNowResources(); // ���� �ڿ�
//    }

//    void Update()
//    {
//        // rŰ�� ������ ��� ������ �ڿ� �߰�
//        if (Input.GetKeyDown(addResourceKey))
//        {
//            AddAllTestResources(resourceAddAmount);
//        }
//    }


//    public bool CheckAllResources(List<ResourceCost> costs)
//    {
//        foreach (var cost in costs)
//        {
//            if (!playerResources.ContainsKey(cost.resourceName) || playerResources[cost.resourceName] < cost.quantity)
//            {
//                Debug.Log($"�ڿ� ����: {cost.resourceName} - �ʿ�: {cost.quantity}, ����: {(playerResources.ContainsKey(cost.resourceName) ? playerResources[cost.resourceName] : 0)}");
//                return false;
//            }
//        }
//        Debug.Log("��� �ڿ��� ����մϴ�.");
//        return true;
//    }

   
    
//    public void ConsumeAllResources(List<ResourceCost> costs)// �ʿ��� ��� �ڿ��� �÷��̾��� (�ӽ� ��ųʸ�)���� �Ҹ�
//    {
//        foreach (var cost in costs)
//        {
           
//            if (playerResources.ContainsKey(cost.resourceName))
//            {
//                playerResources[cost.resourceName] -= cost.quantity;
//                Debug.Log($"   - {cost.resourceName} {cost.quantity}�� �Ҹ�.");
//            }
//            else
//            {
//                Debug.LogWarning($"�ڿ�����");
//            }
//        }
//        LogNowResources(); // �Ҹ� �� ���� ���� �ڿ�
//    }

    
//    public void LogNowResources()
//    {
//        string resLog = "ConsumeItem: ���� ���� �ڿ�: ";
//        foreach (var pair in playerResources)
//        {
//            resLog += $"{pair.Key}: {pair.Value} | ";
//        }
//        Debug.Log(resLog.TrimEnd('|', ' '));
//    }

   
//    private void AddAllTestResources(int amount)//�׽�Ʈ�ڿ��߰�
//    {
//        List<string> resourceNames = new List<string>(playerResources.Keys);
//        foreach (var resName in resourceNames)
//        {
//            playerResources[resName] += amount;
//        }
//        Debug.Log($"ConsumeItem: ��� �ڿ� {amount}���� �߰���.");
//        LogNowResources();
//    }
//}
