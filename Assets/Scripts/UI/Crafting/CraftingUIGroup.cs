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

    // ���â
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
