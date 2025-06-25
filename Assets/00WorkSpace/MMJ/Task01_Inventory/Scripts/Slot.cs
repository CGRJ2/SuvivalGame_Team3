using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Item;
public class Slot : MonoBehaviour, 
    IPointerEnterHandler,           //IPointerEnterHandler - OnPointerEnter - 포인터가 오브젝트에 들어갈 때 호출됩니다.
    IPointerExitHandler,            //IPointerExitHandler - OnPointerExit - 포인터가 오브젝트에서 나올 때 호출됩니다.
    IPointerClickHandler,           //IPointerClickHandler - OnPointerClick - 동일 오브젝트에서 포인터를 누르고 뗄 때 호출됩니다.
    IBeginDragHandler,              //IBeginDragHandler - OnBeginDrag - 드래그가 시작되는 시점에 드래그 대상 오브젝트에서 호출됩니다.
    IDragHandler,                   //IDragHandler - OnDrag - 드래그 오브젝트가 드래그되는 동안 호출됩니다.
    IEndDragHandler,                //IEndDragHandler - OnEndDrag - 드래그가 종료됐을 때 드래그 오브젝트에서 호출됩니다.
    IDropHandler                    //IDropHandler - OnDrop - 드래그를 멈췄을 때 해당 오브젝트에서 호출됩니다.
{
    private Vector3 originPos;
    public ItemType slotType; // Inspector에서 설정해줘야 함
    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 갯수
    public Image itemImage; //아이템의 이미지

    [SerializeField]
    private Text Text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private ItemEffectDatabase theItemEffectDatabase;
    private SlotToolTip theSlot;



    void Start()
    { 
        
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        originPos = transform.position;
    }

    private void SetColor(float _alpha)
    { 
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int _count = 1)
    { 
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            Text_Count.text = itemCount.ToString();
        }
        else 
        {
            Text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        Text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) //IPointerClickHandler - OnPointerClick - 동일 오브젝트에서 포인터를 누르고 뗄 때 호출됩니다.
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 우클릭 상호작용
        {
            if (item != null)
            {
                if (item.itemType == ItemType.Equipment)
                {
                    //장착

                }
                else
                {
                    //소모
                    Debug.Log(item.itemName + "을 사용했습니다");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) //IBeginDragHandler - OnBeginDrag - 드래그가 시작되는 시점에 드래그 대상 오브젝트에서 호출됩니다.
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) //IDragHandler - OnDrag - 드래그 오브젝트가 드래그되는 동안 호출됩니다.
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) //IEndDragHandler - OnEndDrag - 드래그가 종료됐을 때 드래그 오브젝트에서 호출됩니다.
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData) //IDropHandler - OnDrop - 드래그를 멈췄을 때 해당 오브젝트에서 호출됩니다.
    {
        if (DragSlot.instance.dragSlot == null) return;

        Item draggedItem = DragSlot.instance.dragSlot.item;
        if (draggedItem == null) return;

        // 타입 일치 검사 또는 예외 처리 허용
        if (!IsValidItemForSlot(draggedItem.itemType))
        {
            Debug.Log("해당 슬롯에 아이템 타입이 맞지 않습니다.");
            return;
        }

        ChangeSlot();
    }
    private bool IsValidItemForSlot(ItemType itemType)
    {
        // ETC 슬롯은 Equipment, Used 허용
        if (slotType == ItemType.ETC)
        {
            return itemType == ItemType.Equipment || itemType == ItemType.Used;
        }

        // 그 외엔 타입 정확히 일치해야 허용
        return itemType == slotType;
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //IPointerEnterHandler - OnPointerEnter - 포인터가 오브젝트에 들어갈 때 호출됩니다.
    {
        if(item != null)
        theItemEffectDatabase.ShowToolTip(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData) //IPointerExitHandler - OnPointerExit - 포인터가 오브젝트에서 나올 때 호출됩니다.
    {
        theItemEffectDatabase.HideToolTip();
    }


    public int ReduceItem(int amount) //크래프팅을 위한 테스트 코드
    {
        if (itemCount >= amount)
        {
            itemCount -= amount;
            if (itemCount == 0) ClearSlot();
            else SetSlotCount(0); // UI 갱신
            return amount;
        }
        else
        {
            int removed = itemCount;
            ClearSlot();
            return removed;
        }
    }



}
