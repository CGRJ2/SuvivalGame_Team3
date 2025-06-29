using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, // ���� Ȯ�� �뵵
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler // �κ��丮 ��ȣ�ۿ� �뵵
{
    public SlotData slotData;
    //public QuickSlot chainedQuickSlot;
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
            // ���� Ȱ��ȭ ���� �����Կ� �������� ������ => �տ� �� ������ ����
            if (this is QuickSlot)
            {
                if (UIManager.Instance.inventoryUI.quickSlotParent.NowSelectedSlot == this)
                {
                    PlayerManager.Instance.instancePlayer.Status.onHandItem = null;
                }
            }
        }
        // ��������/�Һ������ => ���� ǥ��
        else if (slotData.item.itemType == ItemType.Used || slotData.item.itemType == ItemType.Ingredient)
        {
            itemSprite.sprite = slotData.item.imageSprite;
            countText.text = slotData.currentCount.ToString();
            SetColor(1);
            countTextParent.SetActive(true);
        }
        // ��������/����Ʈ������/��ɾ����� => ���� ǥ�� X
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


    // �κ��丮 ���� & ������ ��Ŭ�� ��ȣ�ۿ� ó��
    //IPointerClickHandler - OnPointerClick - ���� ������Ʈ���� �����͸� ������ �� �� ȣ��˴ϴ�.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // ��Ŭ�� ��ȣ�ۿ�
        {
            // ������ ��Ŭ�� => ������ ����
            if (this is QuickSlot quick)
            {
                ClearSlotView();

                quick.slotData = new SlotData();

                SlotViewUpdate();
            }
            // �������� �ƴ� ������ ���� ��Ŭ��(= �κ��丮 ���� ��Ŭ��) => ������ ���� or ���
            else
            {
                if (slotData.item == null) return;

                // ������ ��ĭ ���� �Ǵ�
                // �κ��丮 �� ���� �� ���� �� ���Կ� ������ ����
                
                // ���� ������ �ֵ��� �����Կ� ����
                if (slotData.item is IEquipable equipable)
                {
                    // �̹� �����Կ� ������ �߰� �ȵ�
                    QuickSlot alreayIn = UIManager.Instance.inventoryUI.quickSlotParent.IsAlreadyInQuickSlot(slotData);
                    if (alreayIn != null) 
                    {
                        Debug.Log($"�̹� �����Կ� ���� :  {alreayIn}, ���� ������ {slotData}");
                        return;
                    }

                    // ��ĭ ������ �߰� �ȵ�
                    QuickSlot emptyQuickSlot = UIManager.Instance.inventoryUI.quickSlotParent.GetEmptyQuickSlot();
                    if (emptyQuickSlot == null)
                    {
                        Debug.Log("������ ��ĭ ����");
                        return;
                    }

                    // �����Կ� ������ ���� �� ���� ������Ʈ
                    emptyQuickSlot.slotData = this.slotData;
                    emptyQuickSlot.SlotViewUpdate();
                }

                // ������ ���� ���� �κ��丮 �� ��� ȿ�� ����
                slotData.item.UseInInventory(slotData);

                // ������ ����� ���� ���� ������Ʈ
                SlotViewUpdate();
            }

        }
    }

    #region �κ��丮 �� ������ Drag & Drop ���

    //IBeginDragHandler - OnBeginDrag - �巡�װ� ���۵Ǵ� ������ �巡�� ��� ������Ʈ���� ȣ��˴ϴ�.
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
        dragSlotInstance.slotView = null;
    }

    //IDropHandler - OnDrop - �巡�׸� ������ �� �ش� ������Ʈ���� ȣ��˴ϴ�.
    public void OnDrop(PointerEventData eventData)
    {
        DragSlotView dragSlotInstance = UIManager.Instance.inventoryUI.dragSlotInstance;

        // ����� �̷������ ���� => this

        if (dragSlotInstance.slotView == null) return;

        // �����Կ��� �巡�� ���̶�� => ������ ������ ��ȯ�� ����ϰ� �̷��������
        if (dragSlotInstance.slotView is QuickSlot draggedQuickSlot)
        {
            // ����� ��ġ�� ������ ������ �������̶�� 
            if (this is QuickSlot droppedQuickSlot)
            {
                // ������ �� ���� ������ ��ü
                SlotData A = droppedQuickSlot.slotData;
                droppedQuickSlot.slotData = draggedQuickSlot.slotData;
                draggedQuickSlot.slotData = A;

                // ���� ���Կ��� �������� �տ� ������Ʈ
                if (UIManager.Instance.inventoryUI.quickSlotParent.NowSelectedSlot == this)
                {
                    if (dragSlotInstance.slotView.slotData.item is IEquipable equipable)
                        equipable.EquipToQuickSlot();
                }

                dragSlotInstance.slotView.SlotViewUpdate(); // => ���� ���� ������Ʈ
            }
        }
        // �κ��丮���� �巡�� ���̶��
        else
        {
            // ����� ��ġ�� ������ ������ �������̶�� => ���� �״�� ���α�
            if (this is QuickSlot quick)
            {
                //QuickSlot AlreadyIn
                if (UIManager.Instance.inventoryUI.quickSlotParent.IsAlreadyInQuickSlot(dragSlotInstance.slotView.slotData))
                {
                    Debug.Log("�̹� �������� �����Կ� �����Ǿ� �ֽ��ϴ�");


                }
                else if (dragSlotInstance.slotView.slotData.item is IEquipable || dragSlotInstance.slotView.slotData.item is IConsumable)
                {
                    // ���� �����Կ� �κ��丮 ������ ����
                    slotData = dragSlotInstance.slotView.slotData;

                    // ���� �������� ���� ���Կ��� �������� �տ� ������Ʈ
                    if (UIManager.Instance.inventoryUI.quickSlotParent.NowSelectedSlot == this 
                        && dragSlotInstance.slotView.slotData.item is IEquipable equipable)
                    {
                        equipable.EquipToQuickSlot();
                    }
                }
            }
            // �������� �ƴ϶��
            // ������ ������ ��ġ ���� �˻�
            // ���� ���� �������̶�� ���� ��ġ��
            else
            {
                // ���� ������ �������̶��
                if (dragSlotInstance.slotView.slotData.item == this.slotData.item)
                {
                    // �� ����
                    MergeSlotData(dragSlotInstance.slotView.slotData, this.slotData);
                }
                // �ٸ� ���� �������̶�� ��ġ ���� (�󽽷� ����)
                else
                {
                    // �� ��ü
                    SlotData A = this.slotData;
                    this.slotData = dragSlotInstance.slotView.slotData;
                    dragSlotInstance.slotView.slotData = A;
                }

                dragSlotInstance.slotView.SlotViewUpdate(); // => ���� ���� ������Ʈ
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
        if (slotData == null) return;
        if (this is QuickSlot) return;
        if (slotData.item != null)
            UIManager.Instance.inventoryUI.tooltip.ShowToolTip(slotData.item, transform.position);
    }

    //IPointerExitHandler - OnPointerExit - �����Ͱ� ������Ʈ���� ���� �� ȣ��˴ϴ�.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotData == null) return;
        if (this is QuickSlot) return;

        UIManager.Instance.inventoryUI.tooltip.HideToolTip();
    }



    #endregion

}
