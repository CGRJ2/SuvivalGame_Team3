using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, // 툴팁 확인 용도
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler // 인벤토리 상호작용 용도
{
    public SlotData slotData;
    //public QuickSlot chainedQuickSlot;
    public Image itemSprite; //아이템의 이미지

    [SerializeField]
    private TMP_Text countText;
    [SerializeField]
    private GameObject countTextParent;

    private void SetColor(float _alpha)
    {
        Color color = itemSprite.color;
        color.a = _alpha;
        itemSprite.color = color;
    }

    public void UpdateInvenChain(SlotData slotData)
    {
        this.slotData = slotData;
    }

    public void QuickSlotHandClear()
    {

    }

    public void SlotViewUpdate()
    {
        if (slotData.item == null)
        {
            ClearSlotView();
            /*if (chainedQuickSlot != null)
            {
                chainedQuickSlot.chainedOriginSlotView = null;
                chainedQuickSlot.slotData = new SlotData();
                chainedQuickSlot = null;
            }*/
            // 현재 활성화 중인 퀵슬롯에 아이템이 없으면 => 손에 든 아이템 제거
            if (this is QuickSlot)
            {
                if (UIManager.Instance.inventoryUI.quickSlotParent.NowSelectedSlot == this)
                {
                    PlayerManager.Instance.instancePlayer.Status.onHandItem = null;
                }
            }
        }
        // 재료아이템/소비아이템 => 갯수 표현
        else if (slotData.item.itemType == ItemType.Used || slotData.item.itemType == ItemType.Ingredient)
        {
            itemSprite.sprite = slotData.item.imageSprite;
            countText.text = slotData.currentCount.ToString();
            SetColor(1);
            countTextParent.SetActive(true);
        }
        // 장비아이템/퀘스트아이템/기능아이템 => 갯수 표현 X
        else
        {
            itemSprite.sprite = slotData.item.imageSprite;
            countText.text = "0";
            SetColor(1);
            countTextParent.SetActive(false);
        }
    }

    public void ClearSlotView()
    {
        itemSprite.sprite = null;
        SetColor(0);
        countText.text = "0";
        countTextParent.SetActive(false);
    }


    // 인벤토리 슬롯 & 퀵슬롯 우클릭 상호작용 처리
    //IPointerClickHandler - OnPointerClick - 동일 오브젝트에서 포인터를 누르고 뗄 때 호출됩니다.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 우클릭 상호작용
        {
            // 퀵슬롯 우클릭 => 퀵슬롯 해제
            if (this is QuickSlot quick)
            {
                ClearSlotView();

                quick.slotData = new SlotData();

                SlotViewUpdate();
            }
            // 퀵슬롯이 아닌 아이템 슬롯 우클릭(= 인벤토리 슬롯 우클릭) => 아이템 장착 or 사용
            else
            {
                if (slotData.item == null) return;

                // 퀵슬롯 빈칸 여부 판단
                // 인벤토리 빈 슬롯 중 가장 앞 슬롯에 데이터 저장
                
                // 장착 가능한 애들은 퀵슬롯에 장착
                if (slotData.item is IEquipable equipable)
                {
                    // 이미 퀵슬롯에 있으면 추가 안됨
                    QuickSlot alreayIn = UIManager.Instance.inventoryUI.quickSlotParent.IsAlreadyInQuickSlot(slotData);
                    if (alreayIn != null) 
                    {
                        Debug.Log($"이미 퀵슬롯에 존재 :  {alreayIn}, 현재 데이터 {slotData}");
                        return;
                    }

                    // 빈칸 없으면 추가 안됨
                    QuickSlot emptyQuickSlot = UIManager.Instance.inventoryUI.quickSlotParent.GetEmptyQuickSlot();
                    if (emptyQuickSlot == null)
                    {
                        Debug.Log("퀵슬롯 빈칸 없음");
                        return;
                    }

                    // 퀵슬롯에 데이터 참조 및 상태 업데이트
                    emptyQuickSlot.slotData = this.slotData;
                    emptyQuickSlot.SlotViewUpdate();
                }

                // 아이템 종류 별로 인벤토리 내 사용 효과 실행
                slotData.item.UseInInventory(slotData);

                // 아이템 사용한 슬롯 상태 업데이트
                SlotViewUpdate();
            }

        }
    }

    #region 인벤토리 내 아이템 Drag & Drop 기능

    //IBeginDragHandler - OnBeginDrag - 드래그가 시작되는 시점에 드래그 대상 오브젝트에서 호출됩니다.
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotData == null) return;
        if (slotData.item != null)
        {
            DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;
            dragSlotInstance.slotView = this;
            dragSlotInstance.DragSetImage(itemSprite);
            dragSlotInstance.transform.position = eventData.position;
        }
    }

    //IDragHandler - OnDrag - 드래그 오브젝트가 드래그되는 동안 호출됩니다.
    public void OnDrag(PointerEventData eventData)
    {
        if (slotData != null)
        {
            DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

            dragSlotInstance.transform.position = eventData.position;
        }
    }

    //IEndDragHandler - OnEndDrag - 드래그가 종료됐을 때 드래그 오브젝트에서 호출됩니다.
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        dragSlotInstance.DropClearImage();
        dragSlotInstance.slotView = null;
    }

    //IDropHandler - OnDrop - 드래그를 멈췄을 때 해당 오브젝트에서 호출됩니다.
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        // 드랍이 이루어지는 슬롯 => this

        if (dragSlotInstance.slotView == null) return;

        // 퀵슬롯에서 드래그 중이라면 => 퀵슬롯 끼리의 교환은 평범하게 이루어지도록
        if (dragSlotInstance.slotView is QuickSlot draggedQuickSlot)
        {
            // 드랍된 위치의 아이템 슬롯이 퀵슬롯이라면 
            if (this is QuickSlot droppedQuickSlot)
            {
                // 퀵슬롯 내 참조 데이터 교체
                SlotData A = droppedQuickSlot.slotData;
                droppedQuickSlot.slotData = draggedQuickSlot.slotData;
                draggedQuickSlot.slotData = A;

                // 현재 슬롯에다 놓았으면 손에 업데이트
                if (UIManager.Instance.inventoryUI.quickSlotParent.NowSelectedSlot == this)
                {
                    if (dragSlotInstance.slotView.slotData.item is IEquipable equipable)
                        equipable.EquipToQuickSlot();
                }

                dragSlotInstance.slotView.SlotViewUpdate(); // => 기존 슬롯 업데이트
            }
        }
        // 인벤토리에서 드래그 중이라면
        else
        {
            // 드랍된 위치의 아이템 슬롯이 퀵슬롯이라면 => 원본 그대로 놔두기
            if (this is QuickSlot quick)
            {
                //QuickSlot AlreadyIn
                if (UIManager.Instance.inventoryUI.quickSlotParent.IsAlreadyInQuickSlot(dragSlotInstance.slotView.slotData))
                {
                    Debug.Log("이미 아이템이 퀵슬롯에 장착되어 있습니다");


                }
                else if (dragSlotInstance.slotView.slotData.item is IEquipable || dragSlotInstance.slotView.slotData.item is IConsumable)
                {
                    // 현재 퀵슬롯에 인벤토리 데이터 참조
                    slotData = dragSlotInstance.slotView.slotData;

                    // 장착 아이템을 현재 슬롯에다 놓았으면 손에 업데이트
                    if (UIManager.Instance.inventoryUI.quickSlotParent.NowSelectedSlot == this 
                        && dragSlotInstance.slotView.slotData.item is IEquipable equipable)
                    {
                        equipable.EquipToQuickSlot();
                    }
                }
            }
            // 퀵슬롯이 아니라면
            // 아이템 데이터 일치 여부 검사
            // 같은 종류 아이템이라면 갯수 합치기
            else
            {
                // 같은 종류의 아이템이라면
                if (dragSlotInstance.slotView.slotData.item == this.slotData.item)
                {
                    // 값 연산
                    MergeSlotData(dragSlotInstance.slotView.slotData, this.slotData);
                }
                // 다른 종류 아이템이라면 위치 변경 (빈슬롯 포함)
                else
                {
                    // 값 교체
                    SlotData A = this.slotData;
                    this.slotData = dragSlotInstance.slotView.slotData;
                    dragSlotInstance.slotView.slotData = A;
                }

                dragSlotInstance.slotView.SlotViewUpdate(); // => 기존 슬롯 업데이트
            }
        }

        SlotViewUpdate();
    }

    // 


    // 자리 교체 (참조 교체가 아닌 ***값 교체***)
    public void ChangeSlotData(SlotData A, SlotData B)
    {
        Item tempItem = A.item;
        int tempCount = A.currentCount;
        int tempMaxCount = A.maxCount;
        A.item = B.item;
        A.currentCount = B.currentCount;
        A.maxCount = B.maxCount;
        B.item = tempItem;
        B.currentCount = tempCount;
        B.maxCount = tempMaxCount;

    }

    // 같은 아이템끼리 드래그 드랍 => 합쳐질 수 있는 수량만큼 합쳐지기
    public void MergeSlotData(SlotData dragSlotData, SlotData dropSlotData)
    {
        int maxCount = dragSlotData.maxCount;

        // 드롭할 공간이 있다면
        if (dropSlotData.currentCount < maxCount)
        {
            int canAddCount = maxCount - dropSlotData.currentCount;

            // 드래그중인 아이템 개수를 합쳤을 때 최대 수량을 넘어간다면
            if (dragSlotData.currentCount > canAddCount)
            {
                dropSlotData.currentCount = maxCount;
                dragSlotData.currentCount -= canAddCount;
            }
            // 드래그중인 아이템 개수를 모두 합칠 수 있을 때
            else
            {
                dropSlotData.currentCount += dragSlotData.currentCount;
                dragSlotData.CleanSlotData();
            }

        }
        // 내가 드롭하는 슬롯에 이미 최대 수량만큼 있다면
        else
        {
            Debug.Log("아무일도 안생겨요");
        }
    }
    #endregion

    #region 마우스 커서를 올릴 때 Tooltip 표시 기능
    //IPointerEnterHandler - OnPointerEnter - 포인터가 오브젝트에 들어갈 때 호출됩니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotData == null) return;
        if (this is QuickSlot) return;
        if (slotData.item != null)
            UIManager.Instance.inventoryUI.tooltip.ShowToolTip(slotData.item, transform.position);
    }

    //IPointerExitHandler - OnPointerExit - 포인터가 오브젝트에서 나올 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotData == null) return;
        if (this is QuickSlot) return;

        UIManager.Instance.inventoryUI.tooltip.HideToolTip();
    }



    #endregion

}
