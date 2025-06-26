using UnityEngine; // 제작시간 수정본
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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

    [Header("진행 바 UI")]
    public Slider progressBar;  // 여기 Scrollbar 추가 (인스펙터에 연결 필요)

    [Header("기타")]
    public Inventory inventory;
    public List<CraftingRecipe> allRecipes;

    private CraftingRecipe selectedRecipe;

    // 제작 진행 관련
    private Coroutine craftingCoroutine = null;
    private bool isCrafting = false;

    void Start()
    {
        ShowRecipeList();
        craftButton.onClick.AddListener(OnClickCraft);
        craftButton.interactable = false; // 시작시 제작 버튼 비활성화
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
            SelectRecipe(selectedRecipe); // UI 다시 갱신
    }

    void CloseCraft()
    {
        go_CraftBase?.SetActive(false);
        StopCrafting(); // 창 닫힐 때 제작 중단 처리
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
                Debug.LogError("RequiredSlot 프리팹에는 Text 컴포넌트 2개가 있어야 합니다.");
            }

            Image itemImage = go.GetComponentInChildren<Image>();
            if (itemImage != null)
                itemImage.sprite = input.itemImage;

            Debug.Log($"[Crafting] HasRequiredItems: {inventory.HasRequiredItems(recipe)} for {recipe.recipeName}");
        }

        craftButton.interactable = inventory.HasRequiredItems(recipe) && !isCrafting; // 제작 중이면 버튼 비활성화
        ResetProgressBar();
    }

    void OnClickCraft()
    {
        if (selectedRecipe == null) return;
        if (!inventory.HasRequiredItems(selectedRecipe)) return;
        if (isCrafting) return; // 이미 제작 중이면 무시

        // 제작 시작
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
                // 창 닫혔으면 제작 중단
                StopCrafting();
                yield break;
            }

            elapsed += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsed / duration);

            yield return null;
        }

        // 제작 완료: 아이템 지급 처리
        inventory.CraftItem(recipe);
        SelectRecipe(recipe); // UI 갱신

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
        Debug.Log("제작 중단");
    }

    void ResetProgressBar()
    {
        if (progressBar != null)
            progressBar.value = 0f;
    }
}