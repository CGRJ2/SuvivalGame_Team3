using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotParent : MonoBehaviour
{
    QuickSlot[] quickSlots;

    [SerializeField] private GameObject selectedStateImage;

    private void Start()
    {
        quickSlots = transform.GetComponentsInChildren<QuickSlot>();
        foreach(QuickSlot qs in quickSlots)
        {
            qs.slotData = new SlotData();
        }

        UIManager.Instance.inventoryUI.quickSlotParent = this;
    }


    // ���� �����Կ� ����� �������� �̹� ��ϵǾ��ִ��� �Ǵ��ϴ� �Լ�
    public bool IsAlreadyInQuickSlot(SlotData dropedSlotData)
    {
        foreach(QuickSlot quickSlotData in quickSlots)
        {
            if (quickSlotData.slotData == dropedSlotData)
            {
                return true;
            }
        }
        return false;
    }

    // ����ִ� ���� ���� ������ ��ȯ
    public QuickSlot GetEmptyQuickSlot()
    {
        foreach (QuickSlot quickSlot in quickSlots)
        {
            if (quickSlot.slotData == default)
            {
                return quickSlot;
            }
        }

        Debug.Log("������ ����� �����");
        return default;
    }


    // ������ �ν��Ͻ�(or ������ �𵨸��� ������Ʈ)�� �տ� Ȱ��ȭ
    public void SelectQuickSlot(int quickSlotNumber, out Item onHandItem)
    {
        SlotData slotData = quickSlots[quickSlotNumber - 1].slotData;

        if (slotData.item != null)
        {
            onHandItem = slotData.item;
        }
        else onHandItem = null;
    }


    private void SelectedSlot(int nowIndex)
    {
        //���ý������� �̵�
        selectedStateImage.transform.position = quickSlots[nowIndex].transform.position;
    }

    public void UpdateQuickSlotView()
    {
        foreach (QuickSlot qs in quickSlots)
        {
            qs.SlotViewUpdate();
        }
    }
}
