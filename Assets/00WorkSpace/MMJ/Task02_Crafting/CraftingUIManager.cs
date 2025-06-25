using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingUIManager : MonoBehaviour
{
    [Header("���� ������ ���")]
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;

    [Header("���� �� ����")]
    public Image iconImage;
    public Text itemNameText;
    public Transform requiredListParent;
    public GameObject requiredSlotPrefab;
    public Button craftButton;

    [Header("��Ÿ")]
    public Inventory inventory;
    public List<CraftingRecipe> allRecipes;

    private CraftingRecipe selectedRecipe;

    void Start()
    {
        ShowRecipeList();
        craftButton.onClick.AddListener(OnClickCraft);
    }

    void ShowRecipeList()
    {
        foreach (Transform child in recipeListParent)
            Destroy(child.gameObject);

        foreach (var recipe in allRecipes)
        {
            GameObject go = Instantiate(recipeButtonPrefab, recipeListParent);
            go.GetComponentInChildren<Text>().text = recipe.recipeName;
            go.GetComponentInChildren<Image>().sprite = recipe.icon;

            go.GetComponent<Button>().onClick.AddListener(() => SelectRecipe(recipe));
        }
    }

    void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;

        iconImage.sprite = recipe.resultItem.itemImage;
        itemNameText.text = recipe.resultItem.itemName;

        // ���� ��� ��� ǥ��
        foreach (Transform child in requiredListParent)
            Destroy(child.gameObject);

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            GameObject go = Instantiate(requiredSlotPrefab, requiredListParent);
            go.GetComponentInChildren<Text>().text = $"{recipe.requiredItems[i].itemName} x {recipe.requiredCounts[i]}";
            go.GetComponentInChildren<Image>().sprite = recipe.requiredItems[i].itemImage;
        }

        craftButton.interactable = inventory.HasRequiredItems(recipe);
    }

    void OnClickCraft()
    {
        if (selectedRecipe == null) return;
        if (!inventory.HasRequiredItems(selectedRecipe)) return;

        inventory.CraftItem(selectedRecipe);
        SelectRecipe(selectedRecipe); // UI �ٽ� ����
    }
}