using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    public InventoryUI inventoryUI;

    public CraftingUI craftingUI;

    public void Init()
    {
        base.SingletonInit();

        
    }
}

[System.Serializable]
public class InventoryUI
{
    public InventoryView inventoryView;
    public SlotToolTip tooltip;
    public DragSlotView dragSlotInstance;
    public QuickSlotParent quickSlotParent;
}

[System.Serializable]
public class CraftingUI
{
    public CraftingUIGroup craftingUIGroup;
    

    // 우측 상세 정보
    public Image iconImage;                 // 선택된 아이템의 아이콘을 표시할 Image 컴포넌트
    public Text itemNameText;               // 선택된 아이템의 이름을 표시할 Text 컴포넌트
    public Transform requiredListParent;    // 제작에 필요한 재료 목록이 표시될 부모 Transform
    public GameObject requiredSlotPrefab;   // 필요한 재료 정보를 표시할 슬롯 프리팹
    public Button craftButton;              // 아이템 제작을 시작하는 버튼
    public Slider progressBar;              // 제작 진행 상태를 표시하는 Slider 컴포넌트

    // 현재 선택된 제작 레시피
    private Item_Recipe selectedRecipe;  

    // 제작 진행 관련
    private Coroutine craftingCoroutine;    // 현재 실행 중인 제작 코루틴에 대한 참조
    private bool isCrafting;                // 현재 제작 중인지 여부를 나타내는

    

    public void PanelOpen()
    {
        craftingUIGroup.UpdateRecipePageData();
        craftingUIGroup.BasePanel.gameObject.SetActive(true);
    }


    
}
