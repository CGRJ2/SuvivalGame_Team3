using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, // ���� Ȯ�� �뵵
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler // �κ��丮 ��ȣ�ۿ� �뵵
{
    public SlotData slotData;
    public Image itemSprite; //�������� �̹���

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
        // ������ ����ִٸ� 
        if (slotData.item == null)
        {
            ClearSlotView();
        }
        // ��������/�Һ������ => ���� ǥ��
        else if (slotData.item.itemType == ItemType.Used || slotData.item.itemType == ItemType.Ingredient)
        {
            itemSprite.sprite = slotData.item.itemImage;
            countText.text = slotData.currentCount.ToString();
            SetColor(1);
            countTextParent.SetActive(true);
        }
        // ��������/����Ʈ������/��ɾ����� => ���� ǥ�� X
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


    #region �κ��丮 �� ������ Drag & Drop ���

    //IPointerClickHandler - OnPointerClick - ���� ������Ʈ���� �����͸� ������ �� �� ȣ��˴ϴ�.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // ��Ŭ�� ��ȣ�ۿ�
        {
            if (slotData.item != null)
            {
                if (slotData.item.itemType == ItemType.Equipment)
                {
                    //����

                }
                else if (slotData.item.itemType == ItemType.Used)
                {
                    //�Ҹ�
                    Debug.Log(slotData.item.itemName + "�� ����߽��ϴ�");
                }
            }
        }
    }

    //IBeginDragHandler - OnBeginDrag - �巡�װ� ���۵Ǵ� ������ �巡�� ��� ������Ʈ���� ȣ��˴ϴ�.
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotData.item != null)
        {
            DragSlotView.instance.dragSlot = this;
            DragSlotView.instance.DragSetImage(itemSprite);
            DragSlotView.instance.transform.position = eventData.position;
        }
    }

    //IDragHandler - OnDrag - �巡�� ������Ʈ�� �巡�׵Ǵ� ���� ȣ��˴ϴ�.
    public void OnDrag(PointerEventData eventData)
    {
        if (slotData.item != null)
        {
            DragSlotView.instance.transform.position = eventData.position;
        }
    }

    //IEndDragHandler - OnEndDrag - �巡�װ� ������� �� �巡�� ������Ʈ���� ȣ��˴ϴ�.
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlotView.instance.DropClearImage();
        DragSlotView.instance.dragSlot = null;
    }

    //IDropHandler - OnDrop - �巡�׸� ������ �� �ش� ������Ʈ���� ȣ��˴ϴ�.
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlotView.instance.dragSlot == null) return;

        // ���� ��� �ִ� ������ ���� (���������� �ӽ� ����)
        SlotView draggedSlot = DragSlotView.instance.dragSlot;
        // ����� �̷������ ���� => this

        if (draggedSlot == null) return;

        // ������ ������ ��ġ ���� �˻�
        // ���� ���� �������̶�� ���� ��ġ��
        if (draggedSlot.slotData.item == this.slotData.item)
        {

        }
        // �ٸ� ���� �������̶�� ��ġ ����
        else
        {
            // slotData ��ȯ
            DragSlotView.instance.dragSlot.slotData = this.slotData;
            this.slotData = draggedSlot.slotData;

            // ���� ĭ���� view ������Ʈ
            DragSlotView.instance.dragSlot.SlotViewUpdate();
            this.SlotViewUpdate();
        }
    }


    #endregion

    #region ���콺 Ŀ���� �ø� �� Tooltip ǥ�� ���
    //IPointerEnterHandler - OnPointerEnter - �����Ͱ� ������Ʈ�� �� �� ȣ��˴ϴ�.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotData.item != null)
            UIManager.Instance.inventoryUI.tooltip.ShowToolTip(slotData.item, transform.position);
    }

    //IPointerExitHandler - OnPointerExit - �����Ͱ� ������Ʈ���� ���� �� ȣ��˴ϴ�.
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.inventoryUI.tooltip.HideToolTip();
    }

    

    #endregion

}
