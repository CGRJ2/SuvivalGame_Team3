using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BaseCamp : MonoBehaviour
//{
        
//        public CampLevel campLevel;
//        public ConsumeItem consumeItem;
//        // public CampItemUnlocker campItemUnlock; // 나중에 아이템 해금 스크립트가 있다면 연결

//        void Awake()
//        {
            
//            if (campLevel == null) campLevel = GetComponent<CampLevel>();
//            if (consumeItem == null) consumeItem = GetComponent<ConsumeItem>();
//            // if (campItemUnlocker == null) campItemUnlock = GetComponent<CampItemUnlock>();


//            // CampLevel의 레벨업 성공 이벤트를 구독
//            if (campLevel != null)
//            {
//                campLevel.OnLevelUpSuccess += HandleLevelUpSuccess;
//            }
//            else
//            {
//                Debug.LogError("CampManager: CampLevel 컴포넌트를 찾을 수 없습니다");
//            }

//            if (consumeItem == null)
//            {
//                Debug.LogError("CampManager: ConsumeItem 컴포넌트를 찾을 수 없습니다");
//            }
//        }

//        void OnDestroy()
//        {
            
//            if (campLevel != null)
//            {
//                campLevel.OnLevelUpSuccess -= HandleLevelUpSuccess;
//            }
//        }

//        void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.Alpha0)) // 임시 레벨업  (0 키)
//            {
//                RequestCampLevelUp();
//            }
//        }

        
//        public void RequestCampLevelUp()
//        {
//            if (campLevel == null || consumeItem == null)
//            {
//                Debug.LogError("CampManager: CampLevel 또는 ConsumeItem 컴포넌트가 연결확인");
//                return;
//            }

            
//            if (campLevel.IsCampMaxLevel())
//            {
//                Debug.Log("CampManager: 베이스캠프가 이미 최대 레벨입니다.");
               
//                return;
//            }

           
//            List<CampLevel.ResourceCost> requiredCosts = campLevel.GetUpgradeCostsForNextLevel();
//            if (requiredCosts == null || requiredCosts.Count == 0)
//            {
//                Debug.LogError("CampManager: 다음 레벨업에 필요한 자원 정보가 설정되지 않았습니다.");
//                return;
//            }

            
//            List<ConsumeItem.ResourceCost> convertedCosts = new List<ConsumeItem.ResourceCost>();
//            foreach (var cost in requiredCosts)
//            {
//                convertedCosts.Add(new ConsumeItem.ResourceCost { resourceName = cost.resourceName, quantity = cost.quantity });
//            }


//            if (consumeItem.CheckAllResources(convertedCosts))
//            {
               
//                consumeItem.ConsumeAllResources(convertedCosts);

               
//                campLevel.LevelUp(); // 이 안에서 OnLevelUpSuccess 이벤트
//            }
//            else
//            {
                
//                Debug.Log("CampManager: 레벨업에 필요한 자원이 부족합니다.");
               
//            }
//        }

       
//        private void HandleLevelUpSuccess()
//        {
//            Debug.Log($"CampManager: 베이스캠프 레벨업 성공! 현재 레벨: {campLevel.CurrentLevel}");
//            //  다른 작업을 트리거
            
//        }
//    }

