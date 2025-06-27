using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingUIManager : MonoBehaviour
{
    public static bool craftActivated = false;

    [SerializeField] private GameObject go_CraftBase;

    [Header("좌측 레시피 목록")]
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;

    [Header("우측 상세 정보")]
    public Image iconImage;
    public Text itemNameText;
    public Transform requiredListParent;
    public GameObject requiredSlotPrefab;
    public Button craftButton;

    [Header("기타")]
    public Inventory inventory;
    public List<CraftingRecipe> allRecipes;

    private CraftingRecipe selectedRecipe;

    void Start()
    {
        ShowRecipeList();
        craftButton.onClick.AddListener(OnClickCraft);
    }

    private void Update()
    {
        TryOpenCraft();
    }

    void TryOpenCraft()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            craftActivated = !craftActivated;

            if (craftActivated)
                OpenCraft();
            else
                CloseCraft();
        }
    }

    void OpenCraft()
    {
        go_CraftBase.SetActive(true);
    }

    void CloseCraft()
    {
        go_CraftBase?.SetActive(false);
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

        // 우측 재료 목록 표시
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
        SelectRecipe(selectedRecipe); // UI 다시 갱신
    }
}