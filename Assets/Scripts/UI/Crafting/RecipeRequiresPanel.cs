using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class RecipeRequiresPanel : MonoBehaviour
{
    [SerializeField] Transform requiredListParent;
    RequiresSlotView[] requiresSlotViews;
    [SerializeField] Color textColor_requiresSufficient;
    [SerializeField] Color textColor_requiresInsufficient;
    [SerializeField] Sprite blockedRecipeSprite;

    // ��� ������ ����
    [SerializeField] Image resultItemImage;
    [SerializeField] TMP_Text resultItemNameText;
    [SerializeField] TMP_Text resultItemTypeText;
    [SerializeField] TMP_Text resultItemCraftTimeText;
    [SerializeField] TMP_Text resultItemDescriptionText;
    public void Init()
    {
        requiresSlotViews = requiredListParent.GetComponentsInChildren<RequiresSlotView>();
    }

    // ��� ������ ������ View ������Ʈ (��밡���� ������ ���� ��)
    public void ShowRecipeRequiresView(Item_Recipe item_Recipe)
    {
        resultItemImage.sprite = item_Recipe.imageSprite;
        resultItemNameText.text = item_Recipe.RecipeData.resultItem.itemName;
        resultItemTypeText.text = item_Recipe.itemType.ToString();
        //resultItemCraftTimeText.text = $"{Mathf.FloorToInt(item_Recipe.RecipeData.craftDuration)}s";
        resultItemCraftTimeText.text = $"{item_Recipe.RecipeData.craftDuration}s";
        resultItemDescriptionText.text = item_Recipe.RecipeData.resultItem.description;
    }

    // ��� ������ ������ View ������Ʈ (��� ������ ���� ��)
    public void ShowLockedRecipeRequiresView()
    {
        resultItemImage.sprite = blockedRecipeSprite;
        resultItemNameText.text = "����ִ� ������";
        resultItemTypeText.text = "";
        resultItemCraftTimeText.text = "�� �� ����";
        resultItemDescriptionText.text = "�� �� ����";
    }   
    // ��� ������ �����͸� ��� ���Կ� ������Ʈ & ���� ���� �Ǻ�
    public bool IsRequiresSufficent(Item_Recipe selectedRecipe)
    {
        // ���õ� �������� ���ٸ�
        if (selectedRecipe == null)
        {
            // ����ִ� �����Ƿ� ������ ������Ʈ
            ShowLockedRecipeRequiresView();

            // ��� ��� ���Ե��� ������ ����ְ� ��Ȱ��ȭ
            for (int i = 0; i < requiresSlotViews.Length; i++)
            {
                requiresSlotViews[i].ClearSlotView();
                requiresSlotViews[i].gameObject.SetActive(false);
            }
            return false;
        }

        // ���õ� �������� ������ ����
        bool canCraft = true;

        // ���õ� ������ ����� ������ �����ֱ�
        ShowRecipeRequiresView(selectedRecipe);

        // ���� ������ �������� ������ǵ��� ��ȸ
        for (int i = 0; i < selectedRecipe.RecipeData.requiredItems.Count; i++)
        {
            // ���� ��� ������ �����۰� �ʿ� ����
            Item requiredItem = selectedRecipe.RecipeData.requiredItems[i].item;
            int requiredCount = selectedRecipe.RecipeData.requiredItems[i].count;

            // �κ��丮 �� �ش� ���� ������ ���� ����
            InventoryPresenter playerInventory = PlayerManager.Instance.instancePlayer.Status.inventory;
            int owned = playerInventory.model.GetOwnedItemCount(requiredItem);
            Debug.Log($"{requiredItem.itemName}�� ���� �ν� ���� : {owned}");

            // ��� ���Կ� ���� ��� ���� ������ �־��ֱ�
            requiresSlotViews[i].requiredItemData = requiredItem;
            requiresSlotViews[i].UpdateSlotView(owned, requiredCount, textColor_requiresSufficient, textColor_requiresInsufficient);
            requiresSlotViews[i].gameObject.SetActive(true);

            // ���� �κ��丮�� ������ ����� ���� �����ϸ�
            if (owned < requiredCount)
            {
                // �ϴ� ���� �Ұ� ���·� �����, �ٸ� ��� ���Ե鵵 ��� ������ ������Ʈ�� ����� ��
                canCraft = false;
            }
        }

        // ��� ������ ������ ��� ������ �������� ������ => ������ ���Ե��� ������ ����ְ� ��Ȱ��ȭ
        if (selectedRecipe.RecipeData.requiredItems.Count < requiresSlotViews.Length)
        {
            for (int i = selectedRecipe.RecipeData.requiredItems.Count; i < requiresSlotViews.Length; i++)
            {
                requiresSlotViews[i].ClearSlotView();
                requiresSlotViews[i].gameObject.SetActive(false);
            }
        }

        return canCraft;
    }
}
