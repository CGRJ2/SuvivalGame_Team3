using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotParent : MonoBehaviour
{
    QuickSlot[] quickSlots;

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
    public void SelectQuickSlotEffect(int quickSlotNumber)
    {
        SlotData slotData = quickSlots[quickSlotNumber - 1].slotData;
        Item selectedItem = slotData.item;



        // ��� ȿ���� �ִ� �������� ���� ȿ���� �켱���� ���� (��� ������ & ���� �� �Һ� ������ �Һ������)
        // �Һ� ȿ���� �ִ� �������̶�� ���� �����ڸ��� ���ȿ�� ���� (��� �Һ� ������)

        // ���� ������ �����ۺ��� �Ǻ� => ����
        if (selectedItem.IsCanEquip())
        {
            // ���� ȿ�� ���� (����� ���ݷ� ����, Todo: �Һ� �������̸鼭 ���� ȿ���� �����ϴ� ������ => ���� Ű ���Ĺ��� ����)
            selectedItem.AdjustEquipEffect();
        }
        else if (selectedItem.IsCanConsume())
        {
            // �Һ�
            selectedItem.AdjustConsumeEffect(slotData);

            // �Һ� �� ���� View ������Ʈ
            quickSlots[quickSlotNumber - 1].SlotViewUpdate();
        }
    }

    public void UpdateQuickSlotView()
    {
        foreach (QuickSlot qs in quickSlots)
        {
            qs.SlotViewUpdate();
        }
    }
}
