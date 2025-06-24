using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
//public struct ResourceCost
//{
//    public string resourceName; // 자원의 이름
//    public int quantity;   // 필요한 개수
//}

//public class ConsumeItem : MonoBehaviour
//{
//    private Dictionary<string, int> playerResources = new Dictionary<string, int>(); //인벤토리와 연동시 제거

//    public KeyCode addResourceKey = KeyCode.R;//테스트자원
//    public int resourceAddAmount = 10;

//    void Start()
//    {
//        // 플레이어가 보유한 자원
//        playerResources.Add("CampResource", 0);
//        LogNowResources(); // 현재 자원
//    }

//    void Update()
//    {
//        // r키를 누르면 모든 종류의 자원 추가
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
//                Debug.Log($"자원 부족: {cost.resourceName} - 필요: {cost.quantity}, 보유: {(playerResources.ContainsKey(cost.resourceName) ? playerResources[cost.resourceName] : 0)}");
//                return false;
//            }
//        }
//        Debug.Log("모든 자원이 충분합니다.");
//        return true;
//    }

   
    
//    public void ConsumeAllResources(List<ResourceCost> costs)// 필요한 모든 자원을 플레이어의 (임시 딕셔너리)에서 소모
//    {
//        foreach (var cost in costs)
//        {
           
//            if (playerResources.ContainsKey(cost.resourceName))
//            {
//                playerResources[cost.resourceName] -= cost.quantity;
//                Debug.Log($"   - {cost.resourceName} {cost.quantity}개 소모.");
//            }
//            else
//            {
//                Debug.LogWarning($"자원오류");
//            }
//        }
//        LogNowResources(); // 소모 후 현재 보유 자원
//    }

    
//    public void LogNowResources()
//    {
//        string resLog = "ConsumeItem: 현재 보유 자원: ";
//        foreach (var pair in playerResources)
//        {
//            resLog += $"{pair.Key}: {pair.Value} | ";
//        }
//        Debug.Log(resLog.TrimEnd('|', ' '));
//    }

   
//    private void AddAllTestResources(int amount)//테스트자원추가
//    {
//        List<string> resourceNames = new List<string>(playerResources.Keys);
//        foreach (var resName in resourceNames)
//        {
//            playerResources[resName] += amount;
//        }
//        Debug.Log($"ConsumeItem: 모든 자원 {amount}개씩 추가됨.");
//        LogNowResources();
//    }
//}
