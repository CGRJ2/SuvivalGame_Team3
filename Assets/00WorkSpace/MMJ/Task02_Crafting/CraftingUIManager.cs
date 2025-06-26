using UnityEngine; // ���۽ð� ������
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CraftingUIManager : MonoBehaviour
{
    public static bool craftActivated = false;

    [SerializeField] private GameObject go_CraftBase;

    [Header("���� ������ ���")]
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;

    [Header("���� �� ����")]
    public Image iconImage;
    public Text itemNameText;
    public Transform requiredListParent;
    public GameObject requiredSlotPrefab;
    public Button craftButton;

    [Header("���� �� UI")]
    public Slider progressBar;  // ���� Scrollbar �߰� (�ν����Ϳ� ���� �ʿ�)

    [Header("��Ÿ")]
    public Inventory inventory;
    public List<CraftingRecipe> allRecipes;

    private CraftingRecipe selectedRecipe;

    // ���� ���� ����
    private Coroutine craftingCoroutine = null;
    private bool isCrafting = false;

    void Start()
    {
        ShowRecipeList();
        craftButton.onClick.AddListener(OnClickCraft);
        craftButton.interactable = false; // ���۽� ���� ��ư ��Ȱ��ȭ
        ResetProgressBar();
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
        if (selectedRecipe != null)
            SelectRecipe(selectedRecipe); // UI �ٽ� ����
    }

    void CloseCraft()
    {
        go_CraftBase?.SetActive(false);
        StopCrafting(); // â ���� �� ���� �ߴ� ó��
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

            CraftingRecipe thisRecipe = recipe;

            go.GetComponent<Button>().onClick.AddListener(() => SelectRecipe(thisRecipe));
        }
    }

    void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;

        iconImage.sprite = recipe.resultItem.itemImage;
        itemNameText.text = recipe.resultItem.itemName;

        foreach (Transform child in requiredListParent)
            Destroy(child.gameObject);

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            Item input = recipe.requiredItems[i];
            int required = recipe.requiredCounts[i];
            int owned = inventory.GetItemCount(input);

            GameObject go = Instantiate(requiredSlotPrefab, requiredListParent);

            Text[] texts = go.GetComponentsInChildren<Text>();

            if (texts.Length >= 2)
            {
                Text nameText = texts[0];
                Text countText = texts[1];

                nameText.text = input.itemName;
                countText.text = $"{required} / {owned}";
                countText.color = (owned >= required) ? Color.green : Color.red;
            }
            else
            {
                Debug.LogError("RequiredSlot �����տ��� Text ������Ʈ 2���� �־�� �մϴ�.");
            }

            Image itemImage = go.GetComponentInChildren<Image>();
            if (itemImage != null)
                itemImage.sprite = input.itemImage;

            Debug.Log($"[Crafting] HasRequiredItems: {inventory.HasRequiredItems(recipe)} for {recipe.recipeName}");
        }

        craftButton.interactable = inventory.HasRequiredItems(recipe) && !isCrafting; // ���� ���̸� ��ư ��Ȱ��ȭ
        ResetProgressBar();
    }

    void OnClickCraft()
    {
        if (selectedRecipe == null) return;
        if (!inventory.HasRequiredItems(selectedRecipe)) return;
        if (isCrafting) return; // �̹� ���� ���̸� ����

        // ���� ����
        craftingCoroutine = StartCoroutine(CraftingProcess(selectedRecipe));
    }

    IEnumerator CraftingProcess(CraftingRecipe recipe)
    {
        isCrafting = true;
        craftButton.interactable = false;

        float elapsed = 0f;
        float duration = recipe.craftDuration;

        while (elapsed < duration)
        {
            if (!go_CraftBase.activeSelf)
            {
                // â �������� ���� �ߴ�
                StopCrafting();
                yield break;
            }

            elapsed += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsed / duration);

            yield return null;
        }

        // ���� �Ϸ�: ������ ���� ó��
        inventory.CraftItem(recipe);
        SelectRecipe(recipe); // UI ����

        isCrafting = false;
        craftButton.interactable = inventory.HasRequiredItems(recipe);
        ResetProgressBar();

        craftingCoroutine = null;
    }

    void StopCrafting()
    {
        if (craftingCoroutine != null)
        {
            StopCoroutine(craftingCoroutine);
            craftingCoroutine = null;
        }
        isCrafting = false;
        craftButton.interactable = inventory.HasRequiredItems(selectedRecipe);
        ResetProgressBar();
        Debug.Log("���� �ߴ�");
    }

    void ResetProgressBar()
    {
        if (progressBar != null)
            progressBar.value = 0f;
    }
}