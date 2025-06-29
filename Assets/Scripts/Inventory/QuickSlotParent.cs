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

        UIManager.Instance.inventoryUI.quickSlotParent = this;

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
        NowSelectedSlot = quickSlots[quickSlotNumber];

        if (NowSelectedSlot.slotData.item != null)
        {
            Debug.Log(NowSelectedSlot.slotData.item);
            PlayerManager.Instance.instancePlayer.Status.onHandItem = NowSelectedSlot.slotData.item;
        }
        else PlayerManager.Instance.instancePlayer.Status.onHandItem = null;

        // ��� �Һ����̸�  ���⼭ �Һ� ȿ�� ����

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
