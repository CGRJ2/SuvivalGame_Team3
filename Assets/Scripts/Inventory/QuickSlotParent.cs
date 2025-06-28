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


    // 현재 퀵슬롯에 드롭한 아이템이 이미 등록되어있는지 판단하는 함수
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

    // 비어있는 가장 앞의 퀵슬롯 반환
    public QuickSlot GetEmptyQuickSlot()
    {
        foreach (QuickSlot quickSlot in quickSlots)
        {
            if (quickSlot.slotData == default)
            {
                return quickSlot;
            }
        }

        Debug.Log("퀵슬롯 빈공간 없어요");
        return default;
    }


    // 아이템 인스턴스(or 아이템 모델링을 업데이트)를 손에 활성화
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
        //선택슬롯으로 이동
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
