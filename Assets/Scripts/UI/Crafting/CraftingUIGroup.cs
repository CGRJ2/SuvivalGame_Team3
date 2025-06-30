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

    public void PanelOpen_CraftRecipeList()
    {
        recipeListPage.nowPageIndex = 0;
        recipeListPage.UpdateRecipePageData();
        recipeListPage.gameObject.SetActive(true);
    }

    public void PanelOpen_CraftRequires()
    {
        // 패널이 안 열린 상태라면 열기
        if (!recipeRequires.gameObject.activeSelf)
            recipeRequires.gameObject.SetActive(true);

        // 현재 선택된 레시피의 정보 업데이트 & 충분한 재료가 있다면 버튼 활성화
        craftButton.interactable = recipeRequires.IsRequiresSufficent(currentSelectedRecipe);
    }


    // 레시피 슬롯 누를 때
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

        PanelOpen_CraftRequires();


    }
}
