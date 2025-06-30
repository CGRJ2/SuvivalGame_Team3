using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIGroup : MonoBehaviour
{
    public Item_Recipe currentSelectedRecipe;

    [field: SerializeField] public Transform BasePanel { get; private set; }

    [SerializeField] RecipeListPagePanel recipeListPage;
    [SerializeField] RecipeRequiresPanel recipeRequires;
    [SerializeField] Button craftButton;               




    private void Awake() => Init();
    public void Init()
    {
        UIManager.Instance.craftingUI.craftingUIGroup = this;
        recipeListPage.Init();
        recipeRequires.Init();
    }

    public void OpenPanel_CraftRecipeList()
    {
        recipeListPage.nowPageIndex = 0;
        recipeListPage.UpdateRecipePageData();
        recipeListPage.gameObject.SetActive(true);
    }

    public void OpenPanel_CraftRequires()
    {
        // 패널이 안 열린 상태라면 열기
        if (!recipeRequires.gameObject.activeSelf)
            recipeRequires.gameObject.SetActive(true);

        // 현재 선택된 레시피의 정보 업데이트 & 충분한 재료가 있다면 버튼 활성화
        craftButton.interactable = recipeRequires.IsRequiresSufficent(currentSelectedRecipe);
    }

    public void ClosePanel_Crafting()
    {
        recipeListPage.gameObject.SetActive(false);
        recipeRequires.gameObject.SetActive(false);
        currentSelectedRecipe = null;
    }


    // 레시피 슬롯 누를 때 (레시피 슬롯 버튼 액션에서 참조)
    public void SelectRecipeSlot(RecipeSlotView thisSlot)
    {
        if (thisSlot.recipeItem == null)
        {
            currentSelectedRecipe = null;
        }
        else if (thisSlot.recipeItem.RecipeData.isUnlocked)
        {
            currentSelectedRecipe = thisSlot.recipeItem;
        }

        OpenPanel_CraftRequires();
    }

    // 제작하기 (제작하기 버튼에서 참조)
    public void StartCrafting()
    {
        if (currentSelectedRecipe != null)
        {
            Debug.LogWarning("제작 실행");
            InventoryPresenter playerInventroy = PlayerManager.Instance.instancePlayer.Status.inventory;

            // 인벤토리에 결과 아이템 추가하기
            playerInventroy.AddItem(currentSelectedRecipe.RecipeData.resultItem);

            // 인벤토리에서 재료 아이템들 없애기
            foreach (ItemRequirement itemRequirement in currentSelectedRecipe.RecipeData.requiredItems)
            {
                playerInventroy.RemoveItem(itemRequirement.item, itemRequirement.count);
            }
        }

        // 제작 완료 후, 제작 버튼 활성화 조건 한 번 더 업데이트
        craftButton.interactable = recipeRequires.IsRequiresSufficent(currentSelectedRecipe);
    }
}
