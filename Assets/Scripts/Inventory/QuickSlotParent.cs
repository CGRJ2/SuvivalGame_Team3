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
    public void SelectQuickSlotEffect(int quickSlotNumber)
    {
        SlotData slotData = quickSlots[quickSlotNumber - 1].slotData;
        Item selectedItem = slotData.item;



        // 장비 효과가 있는 아이템은 장착 효과를 우선으로 적용 (장비 아이템 & 장착 후 소비 가능한 소비아이템)
        // 소비 효과만 있는 아이템이라면 슬롯 누르자마자 사용효과 적용 (즉발 소비 아이템)

        // 장착 가능한 아이템부터 판별 => 장착
        if (selectedItem.IsCanEquip())
        {
            // 장착 효과 적용 (무기면 공격력 증가, Todo: 소비 아이템이면서 장착 효과가 존재하는 아이템 => 공격 키 스탠바이 상태)
            selectedItem.AdjustEquipEffect();
        }
        else if (selectedItem.IsCanConsume())
        {
            // 소비
            selectedItem.AdjustConsumeEffect(slotData);

            // 소비 후 슬롯 View 업데이트
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
