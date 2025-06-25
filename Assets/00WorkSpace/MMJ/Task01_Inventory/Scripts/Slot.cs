using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Item;
public class Slot : MonoBehaviour, 
    IPointerEnterHandler,           //IPointerEnterHandler - OnPointerEnter - �����Ͱ� ������Ʈ�� �� �� ȣ��˴ϴ�.
    IPointerExitHandler,            //IPointerExitHandler - OnPointerExit - �����Ͱ� ������Ʈ���� ���� �� ȣ��˴ϴ�.
    IPointerClickHandler,           //IPointerClickHandler - OnPointerClick - ���� ������Ʈ���� �����͸� ������ �� �� ȣ��˴ϴ�.
    IBeginDragHandler,              //IBeginDragHandler - OnBeginDrag - �巡�װ� ���۵Ǵ� ������ �巡�� ��� ������Ʈ���� ȣ��˴ϴ�.
    IDragHandler,                   //IDragHandler - OnDrag - �巡�� ������Ʈ�� �巡�׵Ǵ� ���� ȣ��˴ϴ�.
    IEndDragHandler,                //IEndDragHandler - OnEndDrag - �巡�װ� ������� �� �巡�� ������Ʈ���� ȣ��˴ϴ�.
    IDropHandler                    //IDropHandler - OnDrop - �巡�׸� ������ �� �ش� ������Ʈ���� ȣ��˴ϴ�.
{
    private Vector3 originPos;
    public ItemType slotType; // Inspector���� ��������� ��
    public Item item; //ȹ���� ������
    public int itemCount; //ȹ���� �������� ����
    public Image itemImage; //�������� �̹���

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

    public void OnPointerClick(PointerEventData eventData) //IPointerClickHandler - OnPointerClick - ���� ������Ʈ���� �����͸� ������ �� �� ȣ��˴ϴ�.
    {
        if (eventData.button == PointerEventData.InputButton.Right) // ��Ŭ�� ��ȣ�ۿ�
        {
            if (item != null)
            {
                if (item.itemType == ItemType.Equipment)
                {
                    //����

                }
                else
                {
                    //�Ҹ�
                    Debug.Log(item.itemName + "�� ����߽��ϴ�");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) //IBeginDragHandler - OnBeginDrag - �巡�װ� ���۵Ǵ� ������ �巡�� ��� ������Ʈ���� ȣ��˴ϴ�.
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) //IDragHandler - OnDrag - �巡�� ������Ʈ�� �巡�׵Ǵ� ���� ȣ��˴ϴ�.
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) //IEndDragHandler - OnEndDrag - �巡�װ� ������� �� �巡�� ������Ʈ���� ȣ��˴ϴ�.
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData) //IDropHandler - OnDrop - �巡�׸� ������ �� �ش� ������Ʈ���� ȣ��˴ϴ�.
    {
        if (DragSlot.instance.dragSlot == null) return;

        Item draggedItem = DragSlot.instance.dragSlot.item;
        if (draggedItem == null) return;

        // Ÿ�� ��ġ �˻� �Ǵ� ���� ó�� ���
        if (!IsValidItemForSlot(draggedItem.itemType))
        {
            Debug.Log("�ش� ���Կ� ������ Ÿ���� ���� �ʽ��ϴ�.");
            return;
        }

        ChangeSlot();
    }
    private bool IsValidItemForSlot(ItemType itemType)
    {
        // ETC ������ Equipment, Used ���
        if (slotType == ItemType.ETC)
        {
            return itemType == ItemType.Equipment || itemType == ItemType.Used;
        }

        // �� �ܿ� Ÿ�� ��Ȯ�� ��ġ�ؾ� ���
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

    public void OnPointerEnter(PointerEventData eventData) //IPointerEnterHandler - OnPointerEnter - �����Ͱ� ������Ʈ�� �� �� ȣ��˴ϴ�.
    {
        if(item != null)
        theItemEffectDatabase.ShowToolTip(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData) //IPointerExitHandler - OnPointerExit - �����Ͱ� ������Ʈ���� ���� �� ȣ��˴ϴ�.
    {
        theItemEffectDatabase.HideToolTip();
    }


    public int ReduceItem(int amount) //ũ�������� ���� �׽�Ʈ �ڵ�
    {
        if (itemCount >= amount)
        {
            itemCount -= amount;
            if (itemCount == 0) ClearSlot();
            else SetSlotCount(0); // UI ����
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
