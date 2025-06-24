using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BaseCamp : MonoBehaviour
//{
        
//        public CampLevel campLevel;
//        public ConsumeItem consumeItem;
//        // public CampItemUnlocker campItemUnlock; // ���߿� ������ �ر� ��ũ��Ʈ�� �ִٸ� ����

//        void Awake()
//        {
            
//            if (campLevel == null) campLevel = GetComponent<CampLevel>();
//            if (consumeItem == null) consumeItem = GetComponent<ConsumeItem>();
//            // if (campItemUnlocker == null) campItemUnlock = GetComponent<CampItemUnlock>();


//            // CampLevel�� ������ ���� �̺�Ʈ�� ����
//            if (campLevel != null)
//            {
//                campLevel.OnLevelUpSuccess += HandleLevelUpSuccess;
//            }
//            else
//            {
//                Debug.LogError("CampManager: CampLevel ������Ʈ�� ã�� �� �����ϴ�");
//            }

//            if (consumeItem == null)
//            {
//                Debug.LogError("CampManager: ConsumeItem ������Ʈ�� ã�� �� �����ϴ�");
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
//            if (Input.GetKeyDown(KeyCode.Alpha0)) // �ӽ� ������  (0 Ű)
//            {
//                RequestCampLevelUp();
//            }
//        }

        
//        public void RequestCampLevelUp()
//        {
//            if (campLevel == null || consumeItem == null)
//            {
//                Debug.LogError("CampManager: CampLevel �Ǵ� ConsumeItem ������Ʈ�� ����Ȯ��");
//                return;
//            }

            
//            if (campLevel.IsCampMaxLevel())
//            {
//                Debug.Log("CampManager: ���̽�ķ���� �̹� �ִ� �����Դϴ�.");
               
//                return;
//            }

           
//            List<CampLevel.ResourceCost> requiredCosts = campLevel.GetUpgradeCostsForNextLevel();
//            if (requiredCosts == null || requiredCosts.Count == 0)
//            {
//                Debug.LogError("CampManager: ���� �������� �ʿ��� �ڿ� ������ �������� �ʾҽ��ϴ�.");
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

               
//                campLevel.LevelUp(); // �� �ȿ��� OnLevelUpSuccess �̺�Ʈ
//            }
//            else
//            {
                
//                Debug.Log("CampManager: �������� �ʿ��� �ڿ��� �����մϴ�.");
               
//            }
//        }

       
//        private void HandleLevelUpSuccess()
//        {
//            Debug.Log($"CampManager: ���̽�ķ�� ������ ����! ���� ����: {campLevel.CurrentLevel}");
//            //  �ٸ� �۾��� Ʈ����
            
//        }
//    }

