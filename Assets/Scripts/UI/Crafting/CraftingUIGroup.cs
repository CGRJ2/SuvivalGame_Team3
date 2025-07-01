using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIGroup : MonoBehaviour
{
    public Item_Recipe currentSelectedRecipe;

    [SerializeField] RecipeListPagePanel recipeListPage;
    [SerializeField] RecipeRequiresPanel recipeRequires;
    [SerializeField] CraftingButton craftButton;               




    private void Awake() => Init();
    public void Init()
    {
        UIManager.Instance.craftingGroup = this;
        recipeListPage.Init();
        recipeRequires.Init();
    }

    public void OpenPanel_CraftRecipeList()
    {
        recipeListPage.nowPageIndex = 0;
        recipeListPage.UpdateRecipePageData();

        // ������ �����ߴ� ���� �ʱ�ȭ
        currentSelectedRecipe = null;

        // �г��� �� ���� ���¶�� ����
        // UI �г� ���ÿ� �߰��ϸ� ����
        if (!recipeListPage.gameObject.activeSelf)
            UIManager.Instance.OpenPanel(recipeListPage.gameObject);
    }

    public void OpenPanel_CraftRequires()
    {
        // �г��� �� ���� ���¶�� ����
        // UI �г� ���ÿ� �߰��ϸ� ����
        if (!recipeRequires.gameObject.activeSelf)
            UIManager.Instance.OpenPanel(recipeRequires.gameObject);

        // ���� ���õ� �������� ���� ������Ʈ & ����� ��ᰡ �ִٸ� ��ư Ȱ��ȭ
        UpdateRequiresPanelState();
    }
    public void UpdateRequiresPanelState()
    {
        craftButton.btnSelf.interactable = recipeRequires.IsRequiresSufficent(currentSelectedRecipe);
        if(currentSelectedRecipe != null)
            craftButton.craftingTime = currentSelectedRecipe.RecipeData.craftDuration;
    }


    // ������ ���� ���� �� (������ ���� ��ư �׼ǿ��� ����)
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

    // �����ϱ� (�����ϱ� ��ư���� ����)
    public void StartCrafting()
    {
        if (currentSelectedRecipe != null)
        {
            //Debug.LogWarning("���� ����");
            InventoryPresenter playerInventroy = PlayerManager.Instance.instancePlayer.Status.inventory;

            // �κ��丮�� ��� ������ �߰��ϱ�
            playerInventroy.AddItem(currentSelectedRecipe.RecipeData.resultItem);

            // �κ��丮���� ��� �����۵� ���ֱ�
            foreach (ItemRequirement itemRequirement in currentSelectedRecipe.RecipeData.requiredItems)
            {
                playerInventroy.RemoveItem(itemRequirement.item, itemRequirement.count);
            }
        }

        // ���� �Ϸ� ��, ���� ��ư Ȱ��ȭ ���� �� �� �� ������Ʈ
        UpdateRequiresPanelState();
    }
}
