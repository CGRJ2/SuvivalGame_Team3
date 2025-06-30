using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIGroup : MonoBehaviour
{
    [field: SerializeField] public Transform BasePanel { get; private set; }      
    [SerializeField] Transform recipeListParent;
    [SerializeField] Transform requiredListParent;

    private RecipeSlotView[] recipeSlotViews;
    List<Item_Recipe> nowPageRecipeList = new List<Item_Recipe>();
    public int pageIndex;

    Item_Recipe currentSelectedRecipe;

    // ���â
    [SerializeField] Image resultItemImage;
    [SerializeField] TMP_Text resultItemNameText;



    private void Awake() => Init();
    public void Init()
    {
        UIManager.Instance.craftingUI.craftingUIGroup = this;

        // ���� ����Ʈ
        recipeSlotViews = recipeListParent.GetComponentsInChildren<RecipeSlotView>();

        // ���� ������ �ε����� �ִ� ������ ����Ʈ(ó���� 15�� ���� null)
        for (int i = 0; i < 15; i++)
        {
            nowPageRecipeList.Add(null);
        }
    }


    public void SetCurrentPageRecipeList(int start, int end)
    {
        Item_Recipe[] allRecipeList = BaseCampManager.Instance.baseCampData.allRecipeList;

        for (int i = start; i < end; i++)
        {
            if (allRecipeList.Length <= i)
            {
                nowPageRecipeList[i - pageIndex * i] = null;
            }
            else
            {
                nowPageRecipeList[i - pageIndex * i] = allRecipeList[i];
            }
        }
    }

    public void UpdateRecipePageData()       // ��� ���� �����Ǹ� UI�� ǥ���ϴ� �޼���, // ���� ������ ��ư���� �����ϰ� ���� ����
    {

        if (pageIndex == 0)
        {
            SetCurrentPageRecipeList(0, 15);
        }
        else if (pageIndex == 1)
        {
            SetCurrentPageRecipeList(15, 30);
        }

        for (int i = 0; i < recipeSlotViews.Length; i++)
        {
            if (nowPageRecipeList[i] != null)
            {
                recipeSlotViews[i].gameObject.SetActive(true);

                // �����ִ� �����Ƕ�� => UI�� ������ ��������Ʈ �ֱ�
                if (nowPageRecipeList[i].RecipeData.isUnlocked)
                {
                    recipeSlotViews[i].recipeItem = nowPageRecipeList[i];
                    recipeSlotViews[i].SetRecipeSprite();
                }
                // ����ִ� �����Ƕ�� => UI�� ���� â���� ����
                else
                {
                    recipeSlotViews[i].recipeItem = null;
                    recipeSlotViews[i].SetBlockedSprite();
                }
            }
            else
            {
                recipeSlotViews[i].gameObject.SetActive(false);
            }
        }
    }


    public void SelectRecipe(Item_Recipe recipe)    // Ư�� �����Ǹ� �����ϰ� �ش� �������� �� ������ UI�� ǥ���ϴ� �޼���
    {
        // ���� ���õ� �����Ǹ� ����
        currentSelectedRecipe = recipe;

        // ��� �������� �����ܰ� �̸��� UI�� ǥ��
        resultItemImage.sprite = recipe.imageSprite;
        resultItemNameText.text = recipe.itemName;


        /*for (int i = 0; i < recipe.requiredItems.Length; i++)   // �����ǿ� �ʿ��� ��� ��Ḧ ��ȸ�ϸ� UI�� ǥ��
        {
            Item input = recipe.requiredItems[i];             // ���� ��� �����۰� �ʿ� ����, ���� ������ ������
            int required = recipe.requiredCounts[i];
            int owned = inventory.GetItemCount(input);

            GameObject go = Instantiate(requiredSlotPrefab, requiredListParent);    // ��� ���� �������� �ν��Ͻ�ȭ

            Text[] texts = go.GetComponentsInChildren<Text>();           // ���Կ� �ִ� ��� Text ������Ʈ�� ������

            if (texts.Length >= 2)              // �ּ� 2���� Text ������Ʈ�� �ִ��� Ȯ��
            {
                Text nameText = texts[0];       // ù ��° Text�� ������ �̸��� ǥ��
                Text countText = texts[1];      // �� ��° Text�� ������ ǥ��

                nameText.text = input.itemName; // ������ �̸��� ���� ������ ����
                countText.text = $"{required} / {owned}";
                countText.color = (owned >= required) ? Color.green : Color.red;    // ���� ������ �ʿ� �������� ���ų� ������ ���, �׷��� ������ ���������� ǥ��
            }
            else
            {
                Debug.LogError("RequiredSlot �����տ��� Text ������Ʈ 2���� �־�� �մϴ�.");
            }

            Image itemImage = go.GetComponentInChildren<Image>();
            if (itemImage != null)
                itemImage.sprite = input.imageSprite;

            Debug.Log($"[Crafting] HasRequiredItems: {inventory.HasRequiredItems(recipe)} for {recipe.recipeName}");
        }

        craftButton.interactable = inventory.HasRequiredItems(recipe) && !isCrafting;  // ���� ��ư Ȱ��ȭ ���θ� ����, �ʿ��� ��Ḧ ��� ������ �ְ�, ���� ���� ���� �ƴ� ���� Ȱ��ȭ
        ResetProgressBar();*/
    }
}
