using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter
{
    public InventoryModel model = new InventoryModel(); // ==> DataField

    public InventoryView view; // ==> UI


    // 기본 생성자 => 처음 생성
    public InventoryPresenter()
    {
        this.model = new InventoryModel();
        this.view = UIManager.Instance.InventoryPanel.GetComponent<InventoryView>();
    }

    // 로드 용
    public InventoryPresenter(InventoryModel model)
    {
        this.model = model;
        this.view = UIManager.Instance.InventoryPanel.GetComponent<InventoryView>();
    }

    public void AddItem(Item item, int count = 1)
    {
        // 추가 가능한지 여부 판단
        if (model.CanAddItem(item, count))
        {
            model.AddItem(item, count);
        }
        else
        {
            // 인벤토리 자리 부족 UI 팝업 실행
            // UIManager.[인벤토리 자리가 부족합니다] 활성화
        }
    }
    public void UpdateUI()
    {
        // 1. 인벤토리를 열었을 때
        // 2. 인벤토리 내부에서 드래그 앤 드롭이 발생했을 때
        // 3. 인벤토리가 활성화 되어있는 상태에서 아이템이 추가/제거되었을 때
        // view.뭐시기뭐시기
    }
}
