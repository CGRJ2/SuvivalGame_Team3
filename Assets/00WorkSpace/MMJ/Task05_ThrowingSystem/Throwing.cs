using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [SerializeField] private float throwForce = 10f; // 던지는 힘
    [SerializeField] private float destroyDelay = 3f; // 소멸 시간(초)
    [SerializeField] private Transform throwPosition; // 던지는 위치 (보통 카메라 또는 손 위치)
    [SerializeField] private QuickSlotParent quickSlotParent;
    

    private void Start()
    {
        //quickSlotParent = UIManager.Instance.inventoryGroup.quickSlotParent; // 이상하게 이걸로 하면 오류나서 직접 연결해서 사용했음...

        if (quickSlotParent == null)
        {
            Debug.LogError("quickSlotParent is null!");
            return;
        }

        if (throwPosition == null)
        {
            throwPosition = Camera.main.transform;
            if (throwPosition == null)
            {
                Debug.LogError("Camera.main is null!");
            }
        }
    }

    // Update 메서드에서 입력 처리
    private void Update()
    {

        // UI 요소 위에 마우스가 있는지 확인하고 있다면 던지지 못함
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 마우스 좌클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // item 체크
            if (quickSlotParent.NowSelectedSlot.slotData.item != null)
            {
                ThrowCurrentItem();
            }
        }
    }

    
    public void ThrowCurrentItem() // 현재 선택된 퀵슬롯의 아이템 던지기
    {
        // 현재 선택된 슬롯 확인
        QuickSlot currentSlot = quickSlotParent.NowSelectedSlot;

        // 슬롯에 아이템이 있고 Item_Throwing 타입인지 확인
        if (currentSlot != null && currentSlot.slotData.item != null && currentSlot.slotData.item is Item_Throwing throwingItem)
        {
            // 아이템 프리팹 생성 위치
            Vector3 spawnPosition = throwPosition.position + throwPosition.forward * 0.5f;

            // 아이템의 프리팹 인스턴스화
            GameObject thrownObject = Instantiate(throwingItem.instancePrefab, spawnPosition, throwPosition.rotation);

            // Rigidbody 컴포넌트 가져오기 또는 추가
            Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = thrownObject.AddComponent<Rigidbody>();
            }

            // 앞 방향으로 힘 가하기
            rb.AddForce(throwPosition.forward * throwForce, ForceMode.Impulse);

            // 일정 시간 후 오브젝트 파괴
            Destroy(thrownObject, destroyDelay);

            // Item_Throwing의 OnAttackEffect 메서드 호출 (필요시)
            throwingItem.OnAttackEffect();

            // 아이템 소비 처리
            throwingItem.Consume(currentSlot.slotData);

            // 슬롯 UI 업데이트
            currentSlot.SlotViewUpdate();
        }
    }
}
