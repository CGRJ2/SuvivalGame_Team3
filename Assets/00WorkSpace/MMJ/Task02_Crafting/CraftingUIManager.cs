using UnityEngine; // ���۽ð� ������
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CraftingUIManager : MonoBehaviour
{
    public static bool craftActivated = false;          // ���� UI�� Ȱ��ȭ ���¸� ��Ÿ���� ���� ����

    [SerializeField] private GameObject go_CraftBase;   // �� ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ�Ͽ� ���� UI�� �Ѱ� ��

    [Header("���� ������ ���")]
    public Transform recipeListParent;      // ������ ��ư���� ������ �θ� Transform
    public GameObject recipeButtonPrefab;   // ������ ��ư�� ������ �� ����� ������

    [Header("���� �� ����")]
    public Image iconImage;                 // ���õ� �������� �������� ǥ���� Image ������Ʈ
    public Text itemNameText;               // ���õ� �������� �̸��� ǥ���� Text ������Ʈ
    public Transform requiredListParent;    // ���ۿ� �ʿ��� ��� ����� ǥ�õ� �θ� Transform
    public GameObject requiredSlotPrefab;   // �ʿ��� ��� ������ ǥ���� ���� ������
    public Button craftButton;              // ������ ������ �����ϴ� ��ư

    [Header("���� �� UI")]
    public Slider progressBar;              // ���� ���� ���¸� ǥ���ϴ� Slider ������Ʈ

    [Header("��Ÿ")]
    public Inventory inventory;             // �÷��̾��� �κ��丮�� ���� ����
    public List<CraftingRecipe> allRecipes; // ���ӿ��� ��� ������ ��� ���� ������ ���

    private CraftingRecipe selectedRecipe;  // ���� ���õ� ���� ������

    // ���� ���� ����
    private Coroutine craftingCoroutine = null;     // ���� ���� ���� ���� �ڷ�ƾ�� ���� ����
    private bool isCrafting = false;                // ���� ���� ������ ���θ� ��Ÿ����

    void Start()
    {
        ShowRecipeList();   // ������ ����� UI�� ǥ��
        craftButton.onClick.AddListener(OnClickCraft);  // ���� ��ư�� Ŭ�� �̺�Ʈ �����ʸ� ����մϴ�.
        craftButton.interactable = false; // ���۽� ���� ��ư ��Ȱ��ȭ
        ResetProgressBar(); // ���� �ٸ� �ʱ� ���·� ����
    }

    private void Update()
    {
        TryOpenCraft();     // ���� UI�� ���� �ݴ� �Է��� ����
    }

    void TryOpenCraft()     // Ư�� Ű �Է�(KeyCode.Tab)�� �����Ͽ� ���� UI�� ���
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

    void ShowRecipeList()       // ��� ���� �����Ǹ� UI�� ǥ���ϴ� �޼���, // ���� ������ ��ư���� �����ϰ� ���� ����
    {
        foreach (Transform child in recipeListParent)   // ������ ������ ������ ��ư���� ��� ����
            Destroy(child.gameObject);

        foreach (var recipe in allRecipes)          // ��� �����ǿ� ���� ��ư�� ����
        {
            GameObject go = Instantiate(recipeButtonPrefab, recipeListParent);
            go.GetComponentInChildren<Text>().text = recipe.recipeName;
            go.GetComponentInChildren<Image>().sprite = recipe.icon;

            CraftingRecipe thisRecipe = recipe;

            go.GetComponent<Button>().onClick.AddListener(() => SelectRecipe(thisRecipe));
        }
    }

    void SelectRecipe(CraftingRecipe recipe)    // Ư�� �����Ǹ� �����ϰ� �ش� �������� �� ������ UI�� ǥ���ϴ� �޼���
    {
        selectedRecipe = recipe;                            // ���� ���õ� �����Ǹ� ����

        iconImage.sprite = recipe.resultItem.itemImage;     // ��� �������� �����ܰ� �̸��� UI�� ǥ��
        itemNameText.text = recipe.resultItem.itemName;

        foreach (Transform child in requiredListParent)      // ������ ǥ�õ� �ʿ� ��� ���Ե��� ��� ����
            Destroy(child.gameObject);

        for (int i = 0; i < recipe.requiredItems.Length; i++)   // �����ǿ� �ʿ��� ��� ��Ḧ ��ȸ�ϸ� UI�� ǥ��
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
                itemImage.sprite = input.itemImage;

            Debug.Log($"[Crafting] HasRequiredItems: {inventory.HasRequiredItems(recipe)} for {recipe.recipeName}");
        }

        craftButton.interactable = inventory.HasRequiredItems(recipe) && !isCrafting;  // ���� ��ư Ȱ��ȭ ���θ� ����, �ʿ��� ��Ḧ ��� ������ �ְ�, ���� ���� ���� �ƴ� ���� Ȱ��ȭ
        ResetProgressBar();
    }

    void OnClickCraft()             // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���, ���õ� �����Ƿ� ������ ������ ����
    {
        if (selectedRecipe == null) return;                          // ������� �� �۾� ����
        if (!inventory.HasRequiredItems(selectedRecipe)) return;     // �ʿ��� ��Ḧ ��� ������ ���� ������ ����
        if (isCrafting) return;                                      // �̹� ���� ���̸� ����

        // ���� ����
        craftingCoroutine = StartCoroutine(CraftingProcess(selectedRecipe));     // ���� ���μ��� �ڷ�ƾ�� ����
    }

    IEnumerator CraftingProcess(CraftingRecipe recipe)          // ������ ���� ������ ó���ϴ� �ڷ�ƾ, ���� �ð� ���� ���� �ٸ� ������Ʈ�ϰ�, �Ϸ� �� �������� ����
    {
        isCrafting = true;                      // ���� �� ���·� �����ϰ� ���� ��ư�� ��Ȱ��ȭ
        craftButton.interactable = false;

        float elapsed = 0f;         // ��� �ð��� �� ���� �ð��� �ʱ�ȭ
        float duration = recipe.craftDuration;

        while (elapsed < duration)  // ���� �ð��� �Ϸ�� ������ �ݺ�
        {
            if (!go_CraftBase.activeSelf)   // ���� UI�� �������� Ȯ��
            {
                // â �������� ���� �ߴ�
                StopCrafting();
                yield break;
            }

            elapsed += Time.deltaTime;          // ��� �ð��� ������Ʈ
            progressBar.value = Mathf.Clamp01(elapsed / duration); // ���� ���¸� 0~1 ���̷� �����Ͽ� �ð������� ǥ��

            yield return null;
        }

        // ���� �Ϸ�: ������ ���� ó��
        inventory.CraftItem(recipe);    // ���� �Ϸ�: �κ��丮�� ��� �������� �߰��ϰ� ��� �������� ����
        SelectRecipe(recipe);   // UI�� �����Ͽ� ����� �κ��丮 ���¸� �ݿ�

        isCrafting = false;     // ���� ���¸� �ʱ�ȭ
        craftButton.interactable = inventory.HasRequiredItems(recipe);
        ResetProgressBar();     // ���� �ٸ� �ʱ�ȭ

        craftingCoroutine = null;
    }

    void StopCrafting()     // ���� ���� ������ �ߴ��ϴ� �޼���, // ���� UI�� �����ų� �ٸ� ������ ������ �ߴ��ؾ� �� �� ȣ��
    {
        if (craftingCoroutine != null)  // ���� ���� ���� ���� �ڷ�ƾ�� �ִٸ� ����
        {
            StopCoroutine(craftingCoroutine);
            craftingCoroutine = null;
        }
        isCrafting = false;         // ���� �� ���¸� ����
        craftButton.interactable = inventory.HasRequiredItems(selectedRecipe);       // �ʿ��� ��Ḧ ��� ������ �ִ� ��쿡�� ���� ��ư�� �ٽ� Ȱ��ȭ
        ResetProgressBar();         // ���� �ٸ� �ʱ�ȭ
        Debug.Log("���� �ߴ�");
    }

    void ResetProgressBar()     // ���� ���� �ٸ� �ʱ� ����(0)�� �����ϴ� �޼���
    {
        if (progressBar != null)
            progressBar.value = 0f;
    }
}