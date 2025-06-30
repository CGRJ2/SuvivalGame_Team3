using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIGroup : MonoBehaviour
{
    Item_Recipe currentSelectedRecipe;

    [field: SerializeField] public Transform BasePanel { get; private set; }

    [SerializeField] RecipeListPage recipeListPage;
    //[SerializeField] 


    [SerializeField] Transform requiredListParent;

    // 결과창
    [SerializeField] Image resultItemImage;
    [SerializeField] TMP_Text resultItemNameText;



    private void Awake() => Init();
    public void Init()
    {
        UIManager.Instance.craftingUI.craftingUIGroup = this;
        recipeListPage.Init();
    }

    public void PanelOpen()
    {
        recipeListPage.UpdateRecipePageData();
        BasePanel.gameObject.SetActive(true);
    }


    public void SelectRecipe(Item_Recipe recipe)    // 특정 레시피를 선택하고 해당 레시피의 상세 정보를 UI에 표시하는 메서드
    {
        // 현재 선택된 레시피를 저장
        currentSelectedRecipe = recipe;

        // 결과 아이템의 아이콘과 이름을 UI에 표시
        resultItemImage.sprite = recipe.imageSprite;
        resultItemNameText.text = recipe.itemName;


        /*for (int i = 0; i < recipe.requiredItems.Length; i++)   // 레시피에 필요한 모든 재료를 순회하며 UI에 표시
        {
            Item input = recipe.requiredItems[i];             // 현재 재료 아이템과 필요 수량, 보유 수량을 가져옴
            int required = recipe.requiredCounts[i];
            int owned = inventory.GetItemCount(input);

            GameObject go = Instantiate(requiredSlotPrefab, requiredListParent);    // 재료 슬롯 프리팹을 인스턴스화

            Text[] texts = go.GetComponentsInChildren<Text>();           // 슬롯에 있는 모든 Text 컴포넌트를 가져옴

            if (texts.Length >= 2)              // 최소 2개의 Text 컴포넌트가 있는지 확인
            {
                Text nameText = texts[0];       // 첫 번째 Text는 아이템 이름을 표시
                Text countText = texts[1];      // 두 번째 Text는 수량을 표시

                nameText.text = input.itemName; // 아이템 이름과 수량 정보를 설정
                countText.text = $"{required} / {owned}";
                countText.color = (owned >= required) ? Color.green : Color.red;    // 보유 수량이 필요 수량보다 많거나 같으면 녹색, 그렇지 않으면 빨간색으로 표시
            }
            else
            {
                Debug.LogError("RequiredSlot 프리팹에는 Text 컴포넌트 2개가 있어야 합니다.");
            }

            Image itemImage = go.GetComponentInChildren<Image>();
            if (itemImage != null)
                itemImage.sprite = input.imageSprite;

            Debug.Log($"[Crafting] HasRequiredItems: {inventory.HasRequiredItems(recipe)} for {recipe.recipeName}");
        }

        craftButton.interactable = inventory.HasRequiredItems(recipe) && !isCrafting;  // 제작 버튼 활성화 여부를 설정, 필요한 재료를 모두 가지고 있고, 현재 제작 중이 아닐 때만 활성화
        ResetProgressBar();*/
    }
}
