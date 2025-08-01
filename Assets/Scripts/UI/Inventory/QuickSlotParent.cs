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

        // 첫 초기화 시 맨앞 슬롯 선택중으로
        NowSelectedSlot = quickSlots[0];
    }


    // 현재 퀵슬롯에 드롭한 아이템이 이미 등록되어있는지 판단하는 함수
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

    // 비어있는 가장 앞의 퀵슬롯 반환
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


    // 아이템 인스턴스(or 아이템 모델링을 업데이트)를 손에 활성화
    public void SelectQuickSlot(int quickSlotNumber)
    {
        if (quickSlots[quickSlotNumber].slotData.item != null)
        {
            // 즉발 소비템이면(소비 & 던지기 아이템 아닌,)  여기서 소비 효과 실행, 퀵슬롯 이동 X
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
        //선택슬롯으로 이동
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
