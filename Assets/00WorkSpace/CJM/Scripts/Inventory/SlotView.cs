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
                // ��� ȿ�� ����
                if (slotData.item.itemType == ItemType.Used)
                {
                    slotData.item.Use(this.slotData);
                }
                else
                {
                    slotData.item.Use();

                }

                // ������ ����� ���� ���� ������Ʈ
                SlotViewUpdate();
            }
        }
    }

    //IBeginDragHandler - OnBeginDrag - �巡�װ� ���۵Ǵ� ������ �巡�� ��� ������Ʈ���� ȣ��˴ϴ�.
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

    //IDragHandler - OnDrag - �巡�� ������Ʈ�� �巡�׵Ǵ� ���� ȣ��˴ϴ�.
    public void OnDrag(PointerEventData eventData)
    {
        if (slotData.item != null)
        {
            DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

            dragSlotInstance.transform.position = eventData.position;
        }
    }

    //IEndDragHandler - OnEndDrag - �巡�װ� ������� �� �巡�� ������Ʈ���� ȣ��˴ϴ�.
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        dragSlotInstance.DropClearImage();
        dragSlotInstance.slot = null;
    }

    //IDropHandler - OnDrop - �巡�׸� ������ �� �ش� ������Ʈ���� ȣ��˴ϴ�.
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        if (dragSlotInstance.slot == null) return;

        // ����� �̷������ ���� => this

        if (dragSlotInstance.slot == null) return;

        // ������ ������ ��ġ ���� �˻�
        // ���� ���� �������̶�� ���� ��ġ��
        if (dragSlotInstance.slot.slotData.item == this.slotData.item)
        {
            MergeSlotData(dragSlotInstance.slot.slotData, this.slotData);
        }
        // �ٸ� ���� �������̶�� ��ġ ����
        else
        {
            ChangeSlotData(dragSlotInstance.slot.slotData, this.slotData);
        }

        dragSlotInstance.slot.SlotViewUpdate();
        SlotViewUpdate();
    }

    // �ܼ� �ڸ� ��ü
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

        // �̰ɷ� �� ���� ������Ʈ�ϰ� ������ �Ұž�.
    }

    // ���� �����۳��� �巡�� ��� => ������ �� �ִ� ������ŭ ��������
    public void MergeSlotData(SlotData dragSlotData, SlotData dropSlotData)
    {
        // ����� ������ �ִٸ�
        if (dropSlotData.currentCount < dropSlotData.maxCount)
        {
            int canAddCount = dropSlotData.maxCount - dropSlotData.currentCount;

            // �巡������ ������ ������ ������ �� �ִ� ������ �Ѿ�ٸ�
            if (dragSlotData.currentCount > canAddCount)
            {
                dropSlotData.currentCount = dropSlotData.maxCount;
                dragSlotData.currentCount -= canAddCount;
            }
            // �巡������ ������ ������ ��� ��ĥ �� ���� ��
            else
            {
                dropSlotData.currentCount += dragSlotData.currentCount;
                dragSlotData.CleanSlot();
            }

        }
        // ���� ����ϴ� ���Կ� �̹� �ִ� ������ŭ �ִٸ�
        else
        {
            Debug.Log("�ƹ��ϵ� �Ȼ��ܿ�");
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
