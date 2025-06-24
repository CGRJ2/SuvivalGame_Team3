using System;
using System.Collections.Generic;
using UnityEngine;

//public class CampLevel : MonoBehaviour
//{
    
//    public event Action OnLevelUpSuccess;//외부구독

//    [Header("Level Settings")]
//    [SerializeField] private int campNowLevel = 0; // 현재 베이스캠프의레벨
//    [SerializeField] private int campMaxLevel = 3; // 베이스캠프 최대레벨

   
//    [System.Serializable]
//    public struct ResourceCost
//    {
//        public string resourceName; // 자원의 이름
//        public int quantity;   // 필요한 개수
//    }

    
//    public List<List<ResourceCost>> upgradeCostsByLevel = new List<List<ResourceCost>>();

    
//    public int CurrentLevel => campNowLevel;
//    public int MaxLevel => campMaxLevel;


//    void Start()
//    {
//        Debug.Log("CampLevel: 현재 레벨: " + campNowLevel);

        
//        if (upgradeCostsByLevel == null || upgradeCostsByLevel.Count == 0)
//        {
//            upgradeCostsByLevel = new List<List<ResourceCost>>();

//            // 레벨 0 -> 1 필요 자원
//            upgradeCostsByLevel.Add(new List<ResourceCost>
//            {
               
//                new ResourceCost { resourceName = "CampResource", quantity = 5 }
//            });
//            // 레벨 1 -> 2 필요 자원
//            upgradeCostsByLevel.Add(new List<ResourceCost>
//            {
               
//                new ResourceCost { resourceName = "CampResource", quantity = 10 }
//            });
//            // 레벨 2 -> 3 필요 자원
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
//            Debug.LogWarning("베이스캠프 최대 레벨업 불가.");
//            return;
//        }

//        campNowLevel++;
//        Debug.Log("베이스캠프 레벨업! 현재 레벨: " + campNowLevel);

        
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
//            Debug.LogWarning($"CampLevel: 레벨에 대한 레벨업 비용 정보가 없음");
//            return null; // 또는 빈 리스트 반환
//        }
//    }
//}