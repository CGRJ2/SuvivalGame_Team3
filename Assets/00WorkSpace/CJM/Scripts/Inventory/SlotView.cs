using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, // 툴팁 확인 용도
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler // 인벤토리 상호작용 용도
{
    public SlotData slotData;
    public QuickSlot chainedQuickSlot;
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

    public void SlotViewUpdate()
    {
        // 슬롯의 참조가 비어있다면 => 빈칸이라는 뜻
        if (slotData == null)
        {
            ClearSlotView();
        }
        else if (slotData.item == null)
        {
            ClearSlotView();
        }
        // 재료아이템/소비아이템 => 갯수 표현
        else if (slotData.item.itemType == ItemType.Used || slotData.item.itemType == ItemType.Ingredient)
        {
            itemSprite.sprite = slotData.item.itemImage;
            countText.text = slotData.currentCount.ToString();
            SetColor(1);
            countTextParent.SetActive(true);
        }
        // 장비아이템/퀘스트아이템/기능아이템 => 갯수 표현 X
        else
        {
            itemSprite.sprite = slotData.item.itemImage;
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


    #region 인벤토리 내 아이템 Drag & Drop 기능

    //IPointerClickHandler - OnPointerClick - 동일 오브젝트에서 포인터를 누르고 뗄 때 호출됩니다.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 우클릭 상호작용
        {
            if (slotData != null && !(this is QuickSlot))
            {
                // 장착 가능하면 장착(퀵슬롯 빈자리에 추가)
                if (slotData.item.IsCanEquip())
                {
                    // 퀵슬롯에 추가
                    //UIManager.Instance.inventoryUI.quickSlotParent.

                    slotData.item.AdjustConsumeEffect(this.slotData);
                }
                // 장착 불가능 + 사용 가능 아이템이라면 => 바로 사용효과 적용
                else if (slotData.item.IsCanConsume())
                {
                    slotData.item.AdjustConsumeEffect(this.slotData); 
                }

                // 아이템 사용한 슬롯 상태 업데이트
                SlotViewUpdate();

                // 퀵슬롯 뷰 업데이트
                UIManager.Instance.inventoryUI.quickSlotParent.UpdateQuickSlotView();
            }
        }
    }

    //IBeginDragHandler - OnBeginDrag - 드래그가 시작되는 시점에 드래그 대상 오브젝트에서 호출됩니다.
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotData.item != null)
        {
            DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;
            dragSlotInstance.slot = this;
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
        dragSlotInstance.slot = null;
    }

    //IDropHandler - OnDrop - 드래그를 멈췄을 때 해당 오브젝트에서 호출됩니다.
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        // 드랍이 이루어지는 슬롯 => this
        
        if (dragSlotInstance.slot == null) return;

        // 퀵슬롯에서 드래그 중이라면 => 퀵슬롯 끼리의 교환은 평범하게 이루어지도록
        if(dragSlotInstance.slot is QuickSlot draggedQuickSlot)
        {
            // 드랍된 위치의 아이템 슬롯이 퀵슬롯이라면 
            if (this is QuickSlot droppedQuickSlot)
            {
                // 빈칸이면 빈칸도 바꿔주면 됨
                //ChangeSlotData(dragSlotInstance.slot.slotData, droppedQuickSlot.slotData);

                // 퀵슬롯 내 참조 데이터 교체
                SlotData A = droppedQuickSlot.slotData;
                droppedQuickSlot.slotData = draggedQuickSlot.slotData;
                draggedQuickSlot.slotData = A;

                // 퀵슬롯 내부의 참조 데이터의 원본 경로 교체
                SlotView invenChainDataTemp = droppedQuickSlot.chainedOriginSlotView;
                droppedQuickSlot.chainedOriginSlotView = draggedQuickSlot.chainedOriginSlotView;
                draggedQuickSlot.chainedOriginSlotView = invenChainDataTemp;

                // 퀵슬롯들 각각의 오리진 슬롯 뷰에 quickChainedData 업데이트
                if (droppedQuickSlot.chainedOriginSlotView != null)
                    droppedQuickSlot.chainedOriginSlotView.chainedQuickSlot = droppedQuickSlot;
                if (draggedQuickSlot.chainedOriginSlotView != null)
                    draggedQuickSlot.chainedOriginSlotView.chainedQuickSlot = draggedQuickSlot;


                // 퀵슬롯들도 업데이트
                //UIManager.Instance.inventoryUI.quickSlotParent.UpdateQuickSlotView();

                dragSlotInstance.slot.SlotViewUpdate(); // => 기존 슬롯 업데이트
            }
            // 다른 아무곳이라면 => 퀵슬롯에서 제거
            else 
            {
                dragSlotInstance.slot.slotData.CleanSlotData();
                dragSlotInstance.slot.chainedQuickSlot = null;
                dragSlotInstance.slot.SlotViewUpdate(); // => 기존 슬롯 업데이트
            }
        }
        // 인벤토리에서 드래그 중이라면
        else
        {
            // 드랍된 위치의 아이템 슬롯이 퀵슬롯이라면 => 원본 그대로 놔두기
            if (this is QuickSlot quick)
            {
                if (UIManager.Instance.inventoryUI.quickSlotParent.IsAlreadyInQuickSlot(dragSlotInstance.slot.slotData))
                {
                    Debug.Log("이미 아이템이 퀵슬롯에 장착되어 있습니다");
                }
                else
                {
                    // 현재 퀵슬롯에 인벤토리 데이터 참조
                    slotData = dragSlotInstance.slot.slotData;

                    // 인벤토리에서 드래그 했던 슬롯에 퀵 연결 정보 참조
                    dragSlotInstance.slot.chainedQuickSlot = quick;

                    // 퀵슬롯에는 인벤토리 원본 데이터 연결정보 참조
                    quick.chainedOriginSlotView = dragSlotInstance.slot;
                }
            }
            // 퀵슬롯이 아니라면
            // 아이템 데이터 일치 여부 검사
            // 같은 종류 아이템이라면 갯수 합치기
            else
            {
                // 같은 종류의 아이템이라면
                if (dragSlotInstance.slot.slotData.item == this.slotData.item)
                {
                    // 값 연산
                    MergeSlotData(dragSlotInstance.slot.slotData, this.slotData);
                }
                // 다른 종류 아이템이라면 위치 변경 (빈슬롯 포함)
                else
                {
                    // 값 교체
                    ChangeSlotData(dragSlotInstance.slot.slotData, this.slotData);
                }

                // 퀵슬롯 체인 정보 교환
                QuickSlot quickChainDataTemp = chainedQuickSlot;
                this.chainedQuickSlot = dragSlotInstance.slot.chainedQuickSlot;
                dragSlotInstance.slot.chainedQuickSlot = quickChainDataTemp;

                if (this.chainedQuickSlot != null)
                {
                    this.chainedQuickSlot.UpdateInvenChain(this.slotData);
                    this.chainedQuickSlot.chainedOriginSlotView = this;
                }
                
                if (dragSlotInstance.slot.chainedQuickSlot != null)
                {
                    dragSlotInstance.slot.chainedQuickSlot.UpdateInvenChain(dragSlotInstance.slot.slotData);
                    dragSlotInstance.slot.chainedQuickSlot.chainedOriginSlotView = dragSlotInstance.slot;
                }


                dragSlotInstance.slot.SlotViewUpdate(); // => 기존 슬롯 업데이트
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
        if (this is QuickSlot) return;
        if (slotData.item != null)
            UIManager.Instance.inventoryUI.tooltip.ShowToolTip(slotData.item, transform.position);
    }

    //IPointerExitHandler - OnPointerExit - 포인터가 오브젝트에서 나올 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (this is QuickSlot) return;

        UIManager.Instance.inventoryUI.tooltip.HideToolTip();
    }

    

    #endregion

}
