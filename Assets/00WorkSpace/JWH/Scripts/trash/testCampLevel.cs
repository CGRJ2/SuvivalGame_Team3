using System;
using System.Collections.Generic;
using UnityEngine;

//public class CampLevel : MonoBehaviour
//{
    
//    public event Action OnLevelUpSuccess;//�ܺα���

//    [Header("Level Settings")]
//    [SerializeField] private int campNowLevel = 0; // ���� ���̽�ķ���Ƿ���
//    [SerializeField] private int campMaxLevel = 3; // ���̽�ķ�� �ִ뷹��

   
//    [System.Serializable]
//    public struct ResourceCost
//    {
//        public string resourceName; // �ڿ��� �̸�
//        public int quantity;   // �ʿ��� ����
//    }

    
//    public List<List<ResourceCost>> upgradeCostsByLevel = new List<List<ResourceCost>>();

    
//    public int CurrentLevel => campNowLevel;
//    public int MaxLevel => campMaxLevel;


//    void Start()
//    {
//        Debug.Log("CampLevel: ���� ����: " + campNowLevel);

        
//        if (upgradeCostsByLevel == null || upgradeCostsByLevel.Count == 0)
//        {
//            upgradeCostsByLevel = new List<List<ResourceCost>>();

//            // ���� 0 -> 1 �ʿ� �ڿ�
//            upgradeCostsByLevel.Add(new List<ResourceCost>
//            {
               
//                new ResourceCost { resourceName = "CampResource", quantity = 5 }
//            });
//            // ���� 1 -> 2 �ʿ� �ڿ�
//            upgradeCostsByLevel.Add(new List<ResourceCost>
//            {
               
//                new ResourceCost { resourceName = "CampResource", quantity = 10 }
//            });
//            // ���� 2 -> 3 �ʿ� �ڿ�
//            upgradeCostsByLevel.Add(new List<ResourceCost>
//            {
                
//                new ResourceCost { resourceName = "CampResource", quantity = 15 }
//            });
//        }
       
//    }

    
//    public void LevelUp()
//    {
//        if (campNowLevel >= campMaxLevel)
//        {
//            Debug.LogWarning("���̽�ķ�� �ִ� ������ �Ұ�.");
//            return;
//        }

//        campNowLevel++;
//        Debug.Log("���̽�ķ�� ������! ���� ����: " + campNowLevel);

        
//        OnLevelUpSuccess?.Invoke();
//    }

    
//    public bool IsCampMaxLevel()
//    {
//        return campNowLevel >= campMaxLevel;
//    }

    
//    public List<ResourceCost> GetUpgradeCostsForNextLevel()
//    {
//        int nextLevelIndex = campNowLevel;

//        if (nextLevelIndex < upgradeCostsByLevel.Count)
//        {
//            return upgradeCostsByLevel[nextLevelIndex];
//        }
//        else
//        {
//            Debug.LogWarning($"CampLevel: ������ ���� ������ ��� ������ ����");
//            return null; // �Ǵ� �� ����Ʈ ��ȯ
//        }
//    }
//}