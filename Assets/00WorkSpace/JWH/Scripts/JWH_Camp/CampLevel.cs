using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CampLevel : MonoBehaviour
{
    private int CampNowLevel = 0;// 현재 베이스캠프의레벨
    private int CampMaxLevel = 3; // 베이스캠프 최대레벨

    [System.Serializable] // 인스펙터
    public struct ResourceCost
    {
        public string resourceName; // 자원의 이름
        public int quantity;   // 필요한 개수
    }
   
    public List<List<ResourceCost>> upgradeCostsByLevel; // 각 레벨업에 필요한 자원 목록
    private Dictionary<string, int> playerResources = new Dictionary<string, int>(); //  플레이어가 보유한 자원

    // 테스트 자원수급
    public KeyCode addResourceKey = KeyCode.R;
    public int resourceAddAmount = 10; 


    void Start()
    {
        CampNowLevel = 0;
        Debug.Log("현재 레벨: " + CampNowLevel);

        // 테스트 자원 설정
        playerResources.Add("나무", 0);
        playerResources.Add("돌", 0);
        playerResources.Add("천", 0);   
        LogNowResources(); // 현재 자원 상태

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))//임시 레벨업 기능
        {
            CampLevelUp();
        }

        // 테스트 r키를 누르면 모든 종류의 자원추가
        if (Input.GetKeyDown(addResourceKey))
        {
            AddAllTestResources(resourceAddAmount);
        }
    }

    
    public void CampLevelUp()//레벨상승기능
    {

        if (CampNowLevel >= CampMaxLevel)//최대레벨 도달 했는지 체크
        {
            Debug.Log("베이스캠프가 최대 레벨입니다");
            return;
        }
        CampNowLevel++;
        Debug.Log("베이스캠프 레벨업 현재 레벨: " + CampNowLevel);


        // CampNowLevel이 0일 때 upgradeCostsByLevel[0]
        // CampNowLevel이 0 -> 1로 갈 때 upgradeCostsByLevel[0]
        // CampNowLevel이 1 -> 2로 갈 때 upgradeCostsByLevel[1]
        if (upgradeCostsByLevel == null || CampNowLevel >= upgradeCostsByLevel.Count) // 레벨업에 필요한 자원 목록
        {
            Debug.LogError("자원부족");
            return;
        }

        List<ResourceCost> requiredCosts = upgradeCostsByLevel[CampNowLevel];

        
        if (!CheckAllResources(requiredCosts)) //자원 확인
        {
            Debug.Log("레벨업에 필요한 자원이 부족합니다.");
            LogRequiredResources(requiredCosts); // 부족한 자원 로그 출력
            return;
        }

        
        ConsumeAllResources(requiredCosts); // 자원 소모
        Debug.Log("자원 소모");
        

        CampNowLevel++; // 현재 레벨을 1 증가시킵니다.
        Debug.Log("베이스캠프 레벨업 현재 레벨: " + CampNowLevel);


        LogNowResources();

    }

    
    private bool CheckAllResources(List<ResourceCost> costs) //자원 확인
    {
        foreach (var cost in costs)
        {
            
            if (!playerResources.ContainsKey(cost.resourceName) || playerResources[cost.resourceName] < cost.quantity) // 자원부족시 false 반환
            {
                return false;
            }
        }
        return true;
    }

    
    private void ConsumeAllResources(List<ResourceCost> costs) //자원 소모
    {
        foreach (var cost in costs)
        {
            playerResources[cost.resourceName] -= cost.quantity; ;
            Debug.Log($"   - {cost.resourceName} {cost.quantity}개 소모.");
        }
    }

    
    private void LogNowResources() // 보유 자원 로그
    {
        string resLog = "현재 보유 자원: ";
        foreach (var pair in playerResources)
        {
            resLog += $"{pair.Key}: {pair.Value} | ";
        }
        Debug.Log(resLog.TrimEnd('|', ' ')); 
    }

    
    private void LogRequiredResources(List<ResourceCost> costs) // 필요한 자원 로그
    {
        string reqLog = "필요한 자원: ";
        foreach (var cost in costs)
        {
            reqLog += $"{cost.resourceName}: {cost.quantity} (보유: {(playerResources.ContainsKey(cost.resourceName) ? playerResources[cost.resourceName] : 0)}) | ";
        }
        Debug.Log(reqLog.TrimEnd('|', ' '));
    }

    // 테스트 자원을 추가
    private void AddAllTestResources(int amount)
    {
        List<string> resourceNames = new List<string>(playerResources.Keys);
        foreach (var resName in resourceNames)
        {
            playerResources[resName] += amount;
        }
        Debug.Log($"모든 자원 {amount}개씩 추가됨.");
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