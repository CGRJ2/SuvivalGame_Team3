using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, // ���� Ȯ�� �뵵
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler // �κ��丮 ��ȣ�ۿ� �뵵
{
    public SlotData slotData;
    public QuickSlot chainedQuickSlot;
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

    public void UpdateInvenChain(SlotData slotData)
    {
        this.slotData = slotData;
    }

    public void SlotViewUpdate()
    {
        // ������ ������ ����ִٸ� => ��ĭ�̶�� ��
        if (slotData == null)
        {
            ClearSlotView();
        }
        else if (slotData.item == null)
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
            if (slotData != null && !(this is QuickSlot))
            {
                // ���� �����ϸ� ����(������ ���ڸ��� �߰�)
                if (slotData.item.IsCanEquip())
                {
                    // �����Կ� �߰�
                    //UIManager.Instance.inventoryUI.quickSlotParent.

                    slotData.item.AdjustConsumeEffect(this.slotData);
                }
                // ���� �Ұ��� + ��� ���� �������̶�� => �ٷ� ���ȿ�� ����
                else if (slotData.item.IsCanConsume())
                {
                    slotData.item.AdjustConsumeEffect(this.slotData); 
                }

                // ������ ����� ���� ���� ������Ʈ
                SlotViewUpdate();

                // ������ �� ������Ʈ
                UIManager.Instance.inventoryUI.quickSlotParent.UpdateQuickSlotView();
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
        if (slotData != null)
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

        // ����� �̷������ ���� => this
        
        if (dragSlotInstance.slot == null) return;

        // �����Կ��� �巡�� ���̶�� => ������ ������ ��ȯ�� ����ϰ� �̷��������
        if(dragSlotInstance.slot is QuickSlot draggedQuickSlot)
        {
            // ����� ��ġ�� ������ ������ �������̶�� 
            if (this is QuickSlot droppedQuickSlot)
            {
                // ��ĭ�̸� ��ĭ�� �ٲ��ָ� ��
                //ChangeSlotData(dragSlotInstance.slot.slotData, droppedQuickSlot.slotData);

                // ������ �� ���� ������ ��ü
                SlotData A = droppedQuickSlot.slotData;
                droppedQuickSlot.slotData = draggedQuickSlot.slotData;
                draggedQuickSlot.slotData = A;

                // ������ ������ ���� �������� ���� ��� ��ü
                SlotView invenChainDataTemp = droppedQuickSlot.chainedOriginSlotView;
                droppedQuickSlot.chainedOriginSlotView = draggedQuickSlot.chainedOriginSlotView;
                draggedQuickSlot.chainedOriginSlotView = invenChainDataTemp;

                // �����Ե� ������ ������ ���� �信 quickChainedData ������Ʈ
                if (droppedQuickSlot.chainedOriginSlotView != null)
                    droppedQuickSlot.chainedOriginSlotView.chainedQuickSlot = droppedQuickSlot;
                if (draggedQuickSlot.chainedOriginSlotView != null)
                    draggedQuickSlot.chainedOriginSlotView.chainedQuickSlot = draggedQuickSlot;


                // �����Ե鵵 ������Ʈ
                //UIManager.Instance.inventoryUI.quickSlotParent.UpdateQuickSlotView();

                dragSlotInstance.slot.SlotViewUpdate(); // => ���� ���� ������Ʈ
            }
            // �ٸ� �ƹ����̶�� => �����Կ��� ����
            else 
            {
                dragSlotInstance.slot.slotData.CleanSlotData();
                dragSlotInstance.slot.chainedQuickSlot = null;
                dragSlotInstance.slot.SlotViewUpdate(); // => ���� ���� ������Ʈ
            }
        }
        // �κ��丮���� �巡�� ���̶��
        else
        {
            // ����� ��ġ�� ������ ������ �������̶�� => ���� �״�� ���α�
            if (this is QuickSlot quick)
            {
                if (UIManager.Instance.inventoryUI.quickSlotParent.IsAlreadyInQuickSlot(dragSlotInstance.slot.slotData))
                {
                    Debug.Log("�̹� �������� �����Կ� �����Ǿ� �ֽ��ϴ�");
                }
                else
                {
                    // ���� �����Կ� �κ��丮 ������ ����
                    slotData = dragSlotInstance.slot.slotData;

                    // �κ��丮���� �巡�� �ߴ� ���Կ� �� ���� ���� ����
                    dragSlotInstance.slot.chainedQuickSlot = quick;

                    // �����Կ��� �κ��丮 ���� ������ �������� ����
                    quick.chainedOriginSlotView = dragSlotInstance.slot;
                }
            }
            // �������� �ƴ϶��
            // ������ ������ ��ġ ���� �˻�
            // ���� ���� �������̶�� ���� ��ġ��
            else
            {
                // ���� ������ �������̶��
                if (dragSlotInstance.slot.slotData.item == this.slotData.item)
                {
                    // �� ����
                    MergeSlotData(dragSlotInstance.slot.slotData, this.slotData);
                }
                // �ٸ� ���� �������̶�� ��ġ ���� (�󽽷� ����)
                else
                {
                    // �� ��ü
                    ChangeSlotData(dragSlotInstance.slot.slotData, this.slotData);
                }

                // ������ ü�� ���� ��ȯ
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


                dragSlotInstance.slot.SlotViewUpdate(); // => ���� ���� ������Ʈ
            }
        }
        
        SlotViewUpdate();
    }

    // 
    

    // �ڸ� ��ü (���� ��ü�� �ƴ� ***�� ��ü***)
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

    // ���� �����۳��� �巡�� ��� => ������ �� �ִ� ������ŭ ��������
    public void MergeSlotData(SlotData dragSlotData, SlotData dropSlotData)
    {
        int maxCount = dragSlotData.maxCount;

        // ����� ������ �ִٸ�
        if (dropSlotData.currentCount < maxCount)
        {
            int canAddCount = maxCount - dropSlotData.currentCount;

            // �巡������ ������ ������ ������ �� �ִ� ������ �Ѿ�ٸ�
            if (dragSlotData.currentCount > canAddCount)
            {
                dropSlotData.currentCount = maxCount;
                dragSlotData.currentCount -= canAddCount;
            }
            // �巡������ ������ ������ ��� ��ĥ �� ���� ��
            else
            {
                dropSlotData.currentCount += dragSlotData.currentCount;
                dragSlotData.CleanSlotData();
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
        if (this is QuickSlot) return;
        if (slotData.item != null)
            UIManager.Instance.inventoryUI.tooltip.ShowToolTip(slotData.item, transform.position);
    }

    //IPointerExitHandler - OnPointerExit - �����Ͱ� ������Ʈ���� ���� �� ȣ��˴ϴ�.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (this is QuickSlot) return;

        UIManager.Instance.inventoryUI.tooltip.HideToolTip();
    }

    

    #endregion

}
