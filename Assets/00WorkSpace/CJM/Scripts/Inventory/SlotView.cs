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
                if (slotData.item.itemType == ItemType.Equipment)
                {
                    //장착

                }
                else if (slotData.item.itemType == ItemType.Used)
                {
                    //소모
                    Debug.Log(slotData.item.itemName + "을 사용했습니다");
                }
            }
        }
    }

    //IBeginDragHandler - OnBeginDrag - 드래그가 시작되는 시점에 드래그 대상 오브젝트에서 호출됩니다.
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotData.item != null)
        {
            DragSlotView.instance.dragSlot = this;
            DragSlotView.instance.DragSetImage(itemSprite);
            DragSlotView.instance.transform.position = eventData.position;
        }
    }

    //IDragHandler - OnDrag - 드래그 오브젝트가 드래그되는 동안 호출됩니다.
    public void OnDrag(PointerEventData eventData)
    {
        if (slotData.item != null)
        {
            DragSlotView.instance.transform.position = eventData.position;
        }
    }

    //IEndDragHandler - OnEndDrag - 드래그가 종료됐을 때 드래그 오브젝트에서 호출됩니다.
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlotView.instance.DropClearImage();
        DragSlotView.instance.dragSlot = null;
    }

    //IDropHandler - OnDrop - 드래그를 멈췄을 때 해당 오브젝트에서 호출됩니다.
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlotView.instance.dragSlot == null) return;

        // 현재 들고 있던 아이템 슬롯 (지역변수로 임시 저장)
        SlotView draggedSlot = DragSlotView.instance.dragSlot;
        // 드랍이 이루어지는 슬롯 => this

        if (draggedSlot == null) return;

        // 아이템 데이터 일치 여부 검사
        // 같은 종류 아이템이라면 갯수 합치기
        if (draggedSlot.slotData.item == this.slotData.item)
        {

        }
        // 다른 종류 아이템이라면 위치 변경
        else
        {
            // slotData 교환
            DragSlotView.instance.dragSlot.slotData = this.slotData;
            this.slotData = draggedSlot.slotData;

            // 각각 칸에서 view 업데이트
            DragSlotView.instance.dragSlot.SlotViewUpdate();
            this.SlotViewUpdate();
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
