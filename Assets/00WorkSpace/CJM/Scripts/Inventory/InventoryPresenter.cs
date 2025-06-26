using System;
using UnityEngine;

public class InventoryPresenter 
{
    public InventoryModel model; // ==> DataField

    public InventoryView view; // ==> UI


    // 기본 생성자 => 처음 생성
    public InventoryPresenter()
    {
        this.model = new InventoryModel();
    }

    // 로드용 함수
    public void SetModel(InventoryModel model)
    {
        this.model = model;
    }

    // 씬전환 시 인벤토리 캔버스가 달라질 때 적용. UIManager에 inventoryView 값에 구독
    public void SetView(InventoryView view)
    {
        this.view = view;
        view.CurrentTab.Subscribe(UpdateSlotsToCurrentTab);
    }

    public void AddItem(Item item, int count = 1)
    {
        // 추가 가능한지 여부 판단
        if (model.CanAddItem(item, count))
        {
            model.AddItem(item, count);
            // ui 반영
            UpdateUI();
        }
        else
        {
            // 인벤토리 자리 부족 UI 팝업 실행
            // UIManager.[인벤토리 자리가 부족합니다] 활성화
        }
    }

    public void UpdateUI()
    {
        if (view == null) SetView(UIManager.Instance.inventoryUI.inventoryView);
        // 1. 인벤토리를 열었을 때
        // 2. 인벤토리 내부에서 드래그 앤 드롭이 발생했을 때
        // 3. 인벤토리가 활성화 되어있는 상태에서 아이템이 추가/제거되었을 때
        UpdateSlotsToCurrentTab(view.CurrentTab.Value);
    }

    public void UpdateSlotsToCurrentTab(ItemType tabType)
    {
        view.UpdateInventorySlotView(model.GetCurrentTabSlots(tabType));
    }


}
