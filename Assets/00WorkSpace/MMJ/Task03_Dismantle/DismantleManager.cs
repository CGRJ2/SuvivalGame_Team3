using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 해체 시스템을 관리하는 스크립트입니다.
/// 플레이어가 인벤토리 아이템을 해체하고, 그에 따른 보상을 획득하는 기능을 담당합니다.
/// </summary>

public class DismantleManager : MonoBehaviour
{
    public static bool dismantleActivated = false;          // 해체 시스템 UI 활성화 상태를 나타내는 정적 변수입니다.
    [SerializeField] private GameObject go_dismantleBase;   // 해체 시스템의 기본 UI 오브젝트, 이 오브젝트를 활성화, 비활성화로 켜고 끕니다.
    [System.Serializable]
    public class DismantleNormalResult
    {
        public Item item;       // 해체 시 획득할 [일반] 아이템
        public int minCount;    // 획득할 아이템의 최소 수량
        public int maxCount;    // 획득할 아이템의 최대 수량
    }

    [System.Serializable]
    public class DismantleRareResult
    {
        public Item item;                           // 해체 시 획득할 [레어] 아이템
        [Range(0f, 1f)] public float probability;   // 아이템 획득 확률
    }

    [Header("일반 결과 아이템")]
    public DismantleNormalResult[] normalResults;   // 해체 시 항상 획득할 일반 아이템 결과 목록

    [Header("레어 결과 아이템")]
    public DismantleRareResult[] rareResults;       // 해체 시 확률적으로 획득할 희귀 아이템 결과 목록

    [Header("금지 아이템 (해체 불가)")]
    public List<string> dismantleBanList;           // 해체할 수 없는 아이템들의 이름 목록 (작성 예시는 MMJ_Test_Scene -> Canvas -> Dismantle 인스펙터에서 참조 확인)

    [Header("UI 참조")]
    public Slot[] dismantleSlots;                   // 인벤토리에서 썼던 슬롯 재활용 (단 슬롯에서 ALL이라는 슬롯 타입을 추가로 만들어서 구현), 슬롯에 있는 아이템들이 해체 대상이 됨
    public Button dismantleButton;                  // 유니티 UI 버튼, 버튼을 클릭하면 해체 로직이 실행

    public Inventory inventory;                     // 인벤토리

    private void Start()
    {
        if (dismantleButton != null)
            dismantleButton.onClick.AddListener(OnClickDismantle); // 해체 버튼이 Inspector를 통해 할당되었다면, 클릭 시 OnClickDismantle 메서드를 호출하도록 리스너를 추가합니다.
    }
    private void Update()
    {
        TryOpenDismantle();     // 매프레임 마다 해체 UI를 열고 닫는 입력을 감지
    }

    void TryOpenDismantle()     // 특정 키 입력(KeyCode.O)을 감지하여 해체 UI를 토글
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            dismantleActivated = !dismantleActivated;

            if (dismantleActivated)
                OpenDismantle();
            else
                CloseDismantle();
        }
    }

    void OpenDismantle()
    {
        go_dismantleBase.SetActive(true);
    }

    void CloseDismantle()
    {
        go_dismantleBase?.SetActive(false);
    }

    private void OnClickDismantle()                         // 해체 버튼 클릭 시 호출되는 메서드, 해체 슬롯에 있는 아이템들을 처리하고 보상을 지급
    {
        foreach (var slot in dismantleSlots)                // 모든 해체 슬롯을 순회
        {
            if (slot.item != null && slot.itemCount > 0)    // 슬롯에 아이템이 있고, 아이템 수량이 0보다 많을 경우에만 처리
            {
                int count = slot.itemCount;                 // 현재 슬롯에 있는 아이템의 수량을 가져옴

                for (int i = 0; i < count; i++)             // 슬롯에 있는 아이템 수량만큼 반복하여 각 아이템을 개별적으로 해체 시도
                {
                    if (dismantleBanList.Contains(slot.item.itemName))      // 현재 아이템의 이름이 해체 금지 목록에 있는지 확인
                    {
                        Debug.Log($"{slot.item.itemName} 은 해체 불가!");
                        continue;
                    }

                    inventory.RemoveItem(slot.item, 1);     // 인벤토리에서 해당 아이템 1개를 제거
                    GiveRewards();                          // 아이템 해체에 따른 보상을 지급
                }

                slot.ClearSlot();                           // 모든 아이템 처리 후 해당 해체 슬롯을 비움
            }
        }
    }

    private void GiveRewards()                              // 해체된 아이템에 대한 보상을 인벤토리에 추가하는 메서드
    {
        foreach (var normal in normalResults)               // 설정된 모든 일반 보상 아이템에 대해 처리
        {
            int count = Random.Range(normal.minCount, normal.maxCount + 1); // 아이템의 최소-최대 수량 범위 내에서 무작위로 수량을 결정
            inventory.AcquireItem(normal.item, count);                      // 인벤토리에 결정된 수량만큼의 아이템을 추가
        }

        foreach (var rare in rareResults)                   // 설정된 모든 희귀 보상 아이템에 대해 처리
        {
            if (Random.value <= rare.probability)           // 확률에 따라 아이템을 획득할 경우
            {
                inventory.AcquireItem(rare.item, 1);        // 인벤토리에 해당 희귀 아이템 1개를 추가
            }
        }
    }
}