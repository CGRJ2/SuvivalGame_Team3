using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, // 툴팁 확인 용도
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler // 인벤토리 상호작용 용도
{
    public SlotData slotData;
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

    public void SlotViewUpdate()
    {
        // 슬롯이 비어있다면 
        if (slotData.item == null)
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
            if (slotData.item != null)
            {
                // 사용 효과 실행
                if (slotData.item.itemType == ItemType.Used)
                {
                    slotData.item.Use(this.slotData);
                }
                else
                {
                    slotData.item.Use();

                }

                // 아이템 사용한 슬롯 상태 업데이트
                SlotViewUpdate();
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
        if (slotData.item != null)
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

        if (dragSlotInstance.slot == null) return;

        // 드랍이 이루어지는 슬롯 => this

        if (dragSlotInstance.slot == null) return;

        // 아이템 데이터 일치 여부 검사
        // 같은 종류 아이템이라면 갯수 합치기
        if (dragSlotInstance.slot.slotData.item == this.slotData.item)
        {
            MergeSlotData(dragSlotInstance.slot.slotData, this.slotData);
        }
        // 다른 종류 아이템이라면 위치 변경
        else
        {
            ChangeSlotData(dragSlotInstance.slot.slotData, this.slotData);
        }

        dragSlotInstance.slot.SlotViewUpdate();
        SlotViewUpdate();
    }

    // 단순 자리 교체
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

        // 이걸로 두 슬롯 업데이트하고 마무리 할거야.
    }

    // 같은 아이템끼리 드래그 드랍 => 합쳐질 수 있는 수량만큼 합쳐지기
    public void MergeSlotData(SlotData dragSlotData, SlotData dropSlotData)
    {
        // 드롭할 공간이 있다면
        if (dropSlotData.currentCount < dropSlotData.maxCount)
        {
            int canAddCount = dropSlotData.maxCount - dropSlotData.currentCount;

            // 드래그중인 아이템 개수를 합쳤을 때 최대 수량을 넘어간다면
            if (dragSlotData.currentCount > canAddCount)
            {
                dropSlotData.currentCount = dropSlotData.maxCount;
                dragSlotData.currentCount -= canAddCount;
            }
            // 드래그중인 아이템 개수를 모두 합칠 수 있을 때
            else
            {
                dropSlotData.currentCount += dragSlotData.currentCount;
                dragSlotData.CleanSlot();
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
        if (slotData.item != null)
            UIManager.Instance.inventoryUI.tooltip.ShowToolTip(slotData.item, transform.position);
    }

    //IPointerExitHandler - OnPointerExit - 포인터가 오브젝트에서 나올 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.inventoryUI.tooltip.HideToolTip();
    }

    

    #endregion

}
