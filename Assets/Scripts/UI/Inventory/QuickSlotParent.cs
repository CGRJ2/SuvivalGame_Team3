using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotParent : MonoBehaviour
{
    QuickSlot[] quickSlots;
    public QuickSlot NowSelectedSlot { get; private set; }
    [SerializeField] private GameObject selectedStateImage;

    private void Start()
    {
        quickSlots = transform.GetComponentsInChildren<QuickSlot>();
        foreach(QuickSlot qs in quickSlots)
        {
            qs.slotData = new SlotData();
        }

        // ù �ʱ�ȭ �� �Ǿ� ���� ����������
        NowSelectedSlot = quickSlots[0];
    }


    // ���� �����Կ� ����� �������� �̹� ��ϵǾ��ִ��� �Ǵ��ϴ� �Լ�
    public QuickSlot IsAlreadyInQuickSlot(SlotData dropedSlotData)
    {
        foreach(QuickSlot quickSlot in quickSlots)
        {
            if (quickSlot.slotData == dropedSlotData)
            {
                return quickSlot;
            }
        }
        return null;
    }

    // ����ִ� ���� ���� ������ ��ȯ
    public QuickSlot GetEmptyQuickSlot()
    {
        foreach (QuickSlot quickSlot in quickSlots)
        {
            if (quickSlot.slotData.item == null)
            {
                return quickSlot;
            }
        }
        return null;
    }


    // ������ �ν��Ͻ�(or ������ �𵨸��� ������Ʈ)�� �տ� Ȱ��ȭ
    public void SelectQuickSlot(int quickSlotNumber)
    {

        



        if (quickSlots[quickSlotNumber].slotData.item != null)
        {
            // ��� �Һ����̸�(�Һ� & ������ ������ �ƴ�,)  ���⼭ �Һ� ȿ�� ����, ������ �̵� X
            if (quickSlots[quickSlotNumber].slotData.item is IConsumable consumable && !(consumable is Item_Throwing))
            {
                quickSlots[quickSlotNumber].slotData.item.UseInInventory(quickSlots[quickSlotNumber].slotData);
                return;
            }
            
            PlayerManager.Instance.instancePlayer.UpdateHandItem(quickSlots[quickSlotNumber].slotData.item);
        }
        else PlayerManager.Instance.instancePlayer.UpdateHandItem(null);

        NowSelectedSlot = quickSlots[quickSlotNumber];

        SelectedImageUpdate();
        //UpdateQuickSlotView();

        NowSelectedSlot.SlotViewUpdate();
    }




    private void SelectedImageUpdate()
    {
        //���ý������� �̵�
        selectedStateImage.GetComponent<RectTransform>().position = NowSelectedSlot.GetComponent<RectTransform>().position;
    }

    public void UpdateQuickSlotView()
    {
        foreach (QuickSlot qs in quickSlots)
        {
            qs.SlotViewUpdate();
        }
    }
}
