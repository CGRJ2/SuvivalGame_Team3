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

    //private ItemEffectDatabase theItemEffectDatabase;
    private SlotToolTip theSlot;

    public bool isDismantleSlot = false; //해체슬롯 구분 변수

    void Start()
    {

        //theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
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

        // 해체 슬롯이면 추가 검사
        if (slotType == ItemType.All) // All 타입은 해체슬롯
        {
            var dismantle = FindObjectOfType<DismantleManager>();
            if (dismantle.dismantleBanList.Contains(draggedItem.itemName))
            {
                Debug.Log($"{draggedItem.itemName} 은 해체할 수 없습니다.");
                return;
            }
        }
        if (isDismantleSlot)
        {
            // 해체 슬롯일 경우, 인벤토리 슬롯에서 아이템을 복사해서 담기
            AddItem(draggedItem, 1); // 수량 1개만 표시 (혹은 원하는 값)
            return;
        }

        // 타입 검사
        if (!IsValidItemForSlot(draggedItem.itemType))
        {
            Debug.Log("해당 슬롯에 아이템 타입이 맞지 않습니다.");
            return;
        }

        // 인벤토리끼리 스왑
        ChangeSlot();
    }
    private bool IsValidItemForSlot(ItemType itemType)
    {
        if (slotType == ItemType.All)
        {
            // All 슬롯은 어떤 아이템 타입이든 허용
            return true;
        }
        else if (slotType == ItemType.ETC)
        {
            // ETC 슬롯은 장비, 소비만 허용
            return itemType == ItemType.Equipment || itemType == ItemType.Used;
        }

        // 기본적으로 타입 일치
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
        /*if (item != null)
            theItemEffectDatabase.ShowToolTip(item, transform.position);*/
    }

    public void OnPointerExit(PointerEventData eventData) //IPointerExitHandler - OnPointerExit - 포인터가 오브젝트에서 나올 때 호출됩니다.
    {
        //theItemEffectDatabase.HideToolTip();
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
