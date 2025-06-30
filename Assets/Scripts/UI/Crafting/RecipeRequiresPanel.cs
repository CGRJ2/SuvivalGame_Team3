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

    // 결과 아이템 슬롯
    [SerializeField] Image resultItemImage;
    [SerializeField] TMP_Text resultItemNameText;
    [SerializeField] TMP_Text resultItemTypeText;
    [SerializeField] TMP_Text resultItemCraftTimeText;
    [SerializeField] TMP_Text resultItemDescriptionText;
    public void Init()
    {
        requiresSlotViews = requiredListParent.GetComponentsInChildren<RequiresSlotView>();
    }

    // 결과 아이템 데이터 View 업데이트 (사용가능한 레시피 선택 시)
    public void ShowRecipeRequiresView(Item_Recipe item_Recipe)
    {
        resultItemImage.sprite = item_Recipe.imageSprite;
        resultItemNameText.text = item_Recipe.RecipeData.resultItem.itemName;
        resultItemTypeText.text = item_Recipe.itemType.ToString();
        //resultItemCraftTimeText.text = $"{Mathf.FloorToInt(item_Recipe.RecipeData.craftDuration)}s";
        resultItemCraftTimeText.text = $"{item_Recipe.RecipeData.craftDuration}s";
        resultItemDescriptionText.text = item_Recipe.RecipeData.resultItem.description;
    }

    // 결과 아이템 데이터 View 업데이트 (잠긴 레시피 선택 시)
    public void ShowLockedRecipeRequiresView()
    {
        resultItemImage.sprite = blockedRecipeSprite;
        resultItemNameText.text = "잠겨있는 레시피";
        resultItemTypeText.text = "";
        resultItemCraftTimeText.text = "알 수 없음";
        resultItemDescriptionText.text = "알 수 없음";
    }   
    // 재료 아이템 데이터를 재료 슬롯에 업데이트 & 제작 가능 판별
    public bool IsRequiresSufficent(Item_Recipe selectedRecipe)
    {
        // 선택된 아이템이 없다면
        if (selectedRecipe == null)
        {
            // 잠겨있는 레시피로 데이터 업데이트
            ShowLockedRecipeRequiresView();

            // 모든 재료 슬롯들의 데이터 비워주고 비활성화
            for (int i = 0; i < requiresSlotViews.Length; i++)
            {
                requiresSlotViews[i].ClearSlotView();
                requiresSlotViews[i].gameObject.SetActive(false);
            }
            return false;
        }

        // 선택된 아이템이 있으면 진행
        bool canCraft = true;

        // 선택된 아이템 결과물 데이터 보여주기
        ShowRecipeRequiresView(selectedRecipe);

        // 현재 레시피 아이템의 재료조건들을 순회
        for (int i = 0; i < selectedRecipe.RecipeData.requiredItems.Count; i++)
        {
            // 현재 재료 조건의 아이템과 필요 수량
            Item requiredItem = selectedRecipe.RecipeData.requiredItems[i].item;
            int requiredCount = selectedRecipe.RecipeData.requiredItems[i].count;

            // 인벤토리 내 해당 조건 아이템 보유 수량
            InventoryPresenter playerInventory = PlayerManager.Instance.instancePlayer.Status.inventory;
            int owned = playerInventory.model.GetOwnedItemCount(requiredItem);
            Debug.Log($"{requiredItem.itemName}의 현재 인식 개수 : {owned}");

            // 재료 슬롯에 현재 재료 조건 데이터 넣어주기
            requiresSlotViews[i].requiredItemData = requiredItem;
            requiresSlotViews[i].UpdateSlotView(owned, requiredCount, textColor_requiresSufficient, textColor_requiresInsufficient);
            requiresSlotViews[i].gameObject.SetActive(true);

            // 현재 인벤토리에 보유한 재료의 수가 부족하면
            if (owned < requiredCount)
            {
                // 일단 제작 불가 상태로 만들고, 다른 재료 슬롯들도 재료 데이터 업데이트는 해줘야 함
                canCraft = false;
            }
        }

        // 재료 조건의 개수가 재료 슬롯의 개수보다 적으면 => 나머지 슬롯들의 데이터 비워주고 비활성화
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
