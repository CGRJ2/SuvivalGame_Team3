using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [SerializeField] private float throwForce = 10f; // 던지는 힘
    [SerializeField] private float destroyDelay = 3f; // 소멸 시간(초)
    [SerializeField] private Transform throwPosition; // 던지는 위치 (보통 카메라 또는 손 위치)
    [SerializeField] private QuickSlotParent quickSlotParent;

    // 힘 조절 관련 변수들
    [Header("Throw Force Control")]
    [SerializeField] private float minThrowForce = 5f;  // 최소 던지는 힘
    [SerializeField] private float maxThrowForce = 30f; // 최대 던지는 힘
    [SerializeField] private float chargeRate = 20f;    // 초당 힘 증가량

    // 궤적 미리보기 관련 변수들
    [Header("Trajectory Prediction")]
    [SerializeField] private LineRenderer trajectoryLine; // Line Renderer 컴포넌트
    [SerializeField] private int linePointCount = 50;     // 궤적을 그릴 점의 개수
    [SerializeField] private float lineTimeStep = 0.1f;   // 각 점 사이의 시간 간격

    private bool isCharging = false;
    private float currentChargeTime = 0f;

    private void Start()
    {
        throwPosition = transform;
        quickSlotParent = UIManager.Instance.inventoryGroup.quickSlotParent;
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

        // 궤적 라인 초기화 (시작 시 숨김)
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }
    }

    // Update 메서드에서 입력 처리
    private void Update()
    {
        // UI 요소 위에 마우스가 있는지 확인하고 있다면 던지지 못함
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            // UI 위에 마우스가 있다면 차징 상태를 해제하고 궤적 숨김
            if (isCharging)
            {
                isCharging = false;
                currentChargeTime = 0f;
                HideTrajectory(); // 궤적 숨김
            }
            return;
        }





        // 현재 선택된 아이템 확인
        QuickSlot currentSlot = quickSlotParent.NowSelectedSlot;
        if (currentSlot == null || currentSlot.slotData.item == null)
            return;

        // 점프패드 아이템인지 확인
        if (currentSlot.slotData.item is Item_JumpPad)
        {
            // 마우스 좌클릭을 누르는 순간 즉시 사용
            if (Input.GetMouseButtonDown(0))
            {
                UseJumpPad();
                return; // 점프패드는 특별 처리하므로 여기서 리턴
            }
        }
        // 일반 투척 아이템 처리 (기존 코드)
        else if (currentSlot.slotData.item is Item_Throwing)
        {
            // 기존 투척 로직 유지...
            // 마우스 좌클릭을 누르는 순간 (차징 시작)
            if (Input.GetMouseButtonDown(0))
            {
                isCharging = true;
                currentChargeTime = 0f;
                ShowTrajectory(); // 궤적 표시
            }
            // 마우스 좌클릭을 누르고 있는 동안 (차징 진행 및 궤적 업데이트)
            else if (isCharging && Input.GetMouseButton(0))
            {
                currentChargeTime += Time.deltaTime;

                // 현재 차징된 힘으로 궤적 업데이트
                float currentForce = Mathf.Clamp(minThrowForce + currentChargeTime * chargeRate, minThrowForce, maxThrowForce);
                UpdateTrajectory(currentForce);
            }
            // 마우스 좌클릭을 떼는 순간 (던지기)
            else if (isCharging && Input.GetMouseButtonUp(0))
            {
                isCharging = false; // 차징 상태 해제

                // 최종 던질 힘 계산
                float finalThrowForce = minThrowForce + currentChargeTime * chargeRate;
                finalThrowForce = Mathf.Clamp(finalThrowForce, minThrowForce, maxThrowForce);

                // 아이템 던지기 함수 호출
                ThrowCurrentItem(finalThrowForce);

                currentChargeTime = 0f; // 차징 시간 초기화
                HideTrajectory(); // 궤적 숨김
            }
        }
    }


    // 점프패드 즉시 사용 메서드
    private void UseJumpPad()
    {
        // 현재 선택된 슬롯 확인
        QuickSlot currentSlot = quickSlotParent.NowSelectedSlot;

        // 슬롯에 아이템이 있고 Item_JumpPad 타입인지 확인
        if (currentSlot != null && currentSlot.slotData.item != null && currentSlot.slotData.item is Item_JumpPad jumpPadItem)
        {
            // 플레이어 위치 가져오기
            GameObject player = PlayerManager.Instance.instancePlayer.gameObject;
            Vector3 playerPosition = player.transform.position;

            // 플레이어 바로 아래에 점프패드 생성 (약간 아래로 오프셋)
            Vector3 spawnPosition = new Vector3(playerPosition.x, playerPosition.y - 0.1f, playerPosition.z);

            // 점프패드 프리팹 인스턴스화
            GameObject jumpPad = Instantiate(jumpPadItem.instancePrefab, spawnPosition, Quaternion.identity);

            // 점프패드가 일정 시간 후 사라지도록 설정
            Destroy(jumpPad, 2f);

            // 아이템 소비 처리
            jumpPadItem.Consume(currentSlot.slotData);

            // 슬롯 UI 업데이트
            currentSlot.SlotViewUpdate();
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
    }


    // 수정된 ThrowCurrentItem 메서드 - 힘 매개변수 추가
    public void ThrowCurrentItem(float force = 10f) // 기본값 설정
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

            // 앞 방향으로 힘 가하기 (전달받은 force 사용)
            rb.AddForce(throwPosition.forward * force, ForceMode.Impulse);

            // 일정 시간 후 오브젝트 파괴
            Destroy(thrownObject, destroyDelay);


            // 슬롯 UI 업데이트
            currentSlot.SlotViewUpdate();

            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
    }

    // --- 궤적 미리보기 관련 함수들 ---

    private void ShowTrajectory()
    {
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = true;
        }
    }

    private void HideTrajectory()
    {
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }
    }

    private void UpdateTrajectory(float currentForce)
    {
        if (trajectoryLine == null) return;

        trajectoryLine.positionCount = linePointCount;

        // 시작 위치를 정확히 설정
        Vector3 startPosition = throwPosition.position;
        // 방향 벡터 확인 - 카메라가 바라보는 방향으로 설정
        Vector3 direction = throwPosition.forward;

        Vector3 startVelocity = direction * currentForce;

        // 첫 번째 점은 정확히 시작 위치에
        trajectoryLine.SetPosition(0, startPosition);

        // 나머지 점들은 시간에 따라 계산
        for (int i = 1; i < linePointCount; i++)
        {
            float time = i * lineTimeStep;
            Vector3 point = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;
            trajectoryLine.SetPosition(i, point);
        }
    }
}