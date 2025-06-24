using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CampLevel : MonoBehaviour
{
    private int CampNowLevel = 0;// ���� ���̽�ķ���Ƿ���
    private int CampMaxLevel = 3; // ���̽�ķ�� �ִ뷹��

    [System.Serializable] // �ν�����
    public struct ResourceCost
    {
        public string resourceName; // �ڿ��� �̸�
        public int quantity;   // �ʿ��� ����
    }
   
    public List<List<ResourceCost>> upgradeCostsByLevel; // �� �������� �ʿ��� �ڿ� ���
    private Dictionary<string, int> playerResources = new Dictionary<string, int>(); //  �÷��̾ ������ �ڿ�

    // �׽�Ʈ �ڿ�����
    public KeyCode addResourceKey = KeyCode.R;
    public int resourceAddAmount = 10; 


    void Start()
    {
        CampNowLevel = 0;
        Debug.Log("���� ����: " + CampNowLevel);

        // �׽�Ʈ �ڿ� ����
        playerResources.Add("����", 0);
        playerResources.Add("��", 0);
        playerResources.Add("õ", 0);   
        LogNowResources(); // ���� �ڿ� ����

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))//�ӽ� ������ ���
        {
            CampLevelUp();
        }

        // �׽�Ʈ rŰ�� ������ ��� ������ �ڿ��߰�
        if (Input.GetKeyDown(addResourceKey))
        {
            AddAllTestResources(resourceAddAmount);
        }
    }

    
    public void CampLevelUp()//������±��
    {

        if (CampNowLevel >= CampMaxLevel)//�ִ뷹�� ���� �ߴ��� üũ
        {
            Debug.Log("���̽�ķ���� �ִ� �����Դϴ�");
            return;
        }
        CampNowLevel++;
        Debug.Log("���̽�ķ�� ������ ���� ����: " + CampNowLevel);


        // CampNowLevel�� 0�� �� upgradeCostsByLevel[0]
        // CampNowLevel�� 0 -> 1�� �� �� upgradeCostsByLevel[0]
        // CampNowLevel�� 1 -> 2�� �� �� upgradeCostsByLevel[1]
        if (upgradeCostsByLevel == null || CampNowLevel >= upgradeCostsByLevel.Count) // �������� �ʿ��� �ڿ� ���
        {
            Debug.LogError("�ڿ�����");
            return;
        }

        List<ResourceCost> requiredCosts = upgradeCostsByLevel[CampNowLevel];

        
        if (!CheckAllResources(requiredCosts)) //�ڿ� Ȯ��
        {
            Debug.Log("�������� �ʿ��� �ڿ��� �����մϴ�.");
            LogRequiredResources(requiredCosts); // ������ �ڿ� �α� ���
            return;
        }

        
        ConsumeAllResources(requiredCosts); // �ڿ� �Ҹ�
        Debug.Log("�ڿ� �Ҹ�");
        

        CampNowLevel++; // ���� ������ 1 ������ŵ�ϴ�.
        Debug.Log("���̽�ķ�� ������ ���� ����: " + CampNowLevel);


        LogNowResources();

    }

    
    private bool CheckAllResources(List<ResourceCost> costs) //�ڿ� Ȯ��
    {
        foreach (var cost in costs)
        {
            
            if (!playerResources.ContainsKey(cost.resourceName) || playerResources[cost.resourceName] < cost.quantity) // �ڿ������� false ��ȯ
            {
                return false;
            }
        }
        return true;
    }

    
    private void ConsumeAllResources(List<ResourceCost> costs) //�ڿ� �Ҹ�
    {
        foreach (var cost in costs)
        {
            playerResources[cost.resourceName] -= cost.quantity; ;
            Debug.Log($"   - {cost.resourceName} {cost.quantity}�� �Ҹ�.");
        }
    }

    
    private void LogNowResources() // ���� �ڿ� �α�
    {
        string resLog = "���� ���� �ڿ�: ";
        foreach (var pair in playerResources)
        {
            resLog += $"{pair.Key}: {pair.Value} | ";
        }
        Debug.Log(resLog.TrimEnd('|', ' ')); 
    }

    
    private void LogRequiredResources(List<ResourceCost> costs) // �ʿ��� �ڿ� �α�
    {
        string reqLog = "�ʿ��� �ڿ�: ";
        foreach (var cost in costs)
        {
            reqLog += $"{cost.resourceName}: {cost.quantity} (����: {(playerResources.ContainsKey(cost.resourceName) ? playerResources[cost.resourceName] : 0)}) | ";
        }
        Debug.Log(reqLog.TrimEnd('|', ' '));
    }

    // �׽�Ʈ �ڿ��� �߰�
    private void AddAllTestResources(int amount)
    {
        List<string> resourceNames = new List<string>(playerResources.Keys);
        foreach (var resName in resourceNames)
        {
            playerResources[resName] += amount;
        }
        Debug.Log($"��� �ڿ� {amount}���� �߰���.");
        LogNowResources();
    }

    public int GetCampNowLevel()
    {
        return CampNowLevel;
    }

    public bool IsCampMaxLevel()
    {
        return CampNowLevel >= CampMaxLevel;
    }
}