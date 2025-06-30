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
        // �г��� �� ���� ���¶�� ����
        if (!recipeRequires.gameObject.activeSelf)
            recipeRequires.gameObject.SetActive(true);

        // ���� ���õ� �������� ���� ������Ʈ & ����� ��ᰡ �ִٸ� ��ư Ȱ��ȭ
        craftButton.interactable = recipeRequires.IsRequiresSufficent(currentSelectedRecipe);
    }


    // ������ ���� ���� ��
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
