using UnityEngine; // 제작시간 수정본
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CraftingUIManager : MonoBehaviour
{
    public static bool craftActivated = false;          // 제작 UI의 활성화 상태를 나타내는 정적 변수

    [SerializeField] private GameObject go_CraftBase;   // 이 오브젝트를 활성화/비활성화하여 제작 UI를 켜고 끔

    [Header("좌측 레시피 목록")]
    public Transform recipeListParent;      // 레시피 버튼들이 생성될 부모 Transform
    public GameObject recipeButtonPrefab;   // 레시피 버튼을 생성할 때 사용할 프리펩

    [Header("우측 상세 정보")]
    public Image iconImage;                 // 선택된 아이템의 아이콘을 표시할 Image 컴포넌트
    public Text itemNameText;               // 선택된 아이템의 이름을 표시할 Text 컴포넌트
    public Transform requiredListParent;    // 제작에 필요한 재료 목록이 표시될 부모 Transform
    public GameObject requiredSlotPrefab;   // 필요한 재료 정보를 표시할 슬롯 프리팹
    public Button craftButton;              // 아이템 제작을 시작하는 버튼

    [Header("진행 바 UI")]
    public Slider progressBar;              // 제작 진행 상태를 표시하는 Slider 컴포넌트

    [Header("기타")]
    public Inventory inventory;             // 플레이어의 인벤토리에 대한 참조
    public List<CraftingRecipe> allRecipes; // 게임에서 사용 가능한 모든 제작 레시피 목록

    private CraftingRecipe selectedRecipe;  // 현재 선택된 제작 레시피

    // 제작 진행 관련
    private Coroutine craftingCoroutine = null;     // 현재 실행 중인 제작 코루틴에 대한 참조
    private bool isCrafting = false;                // 현재 제작 중인지 여부를 나타내는

    void Start()
    {
        ShowRecipeList();   // 레시피 목록을 UI에 표시
        craftButton.onClick.AddListener(OnClickCraft);  // 제작 버튼에 클릭 이벤트 리스너를 등록합니다.
        craftButton.interactable = false; // 시작시 제작 버튼 비활성화
        ResetProgressBar(); // 진행 바를 초기 상태로 리셋
    }

    private void Update()
    {
        TryOpenCraft();     // 제작 UI를 열고 닫는 입력을 감지
    }

    void TryOpenCraft()     // 특정 키 입력(KeyCode.Tab)을 감지하여 제작 UI를 토글
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

    void ShowRecipeList()       // 모든 제작 레시피를 UI에 표시하는 메서드, // 기존 레시피 버튼들을 제거하고 새로 생성
    {
        foreach (Transform child in recipeListParent)   // 기존에 생성된 레시피 버튼들을 모두 제거
            Destroy(child.gameObject);

        foreach (var recipe in allRecipes)          // 모든 레시피에 대해 버튼을 생성
        {
            GameObject go = Instantiate(recipeButtonPrefab, recipeListParent);
            go.GetComponentInChildren<Text>().text = recipe.recipeName;
            go.GetComponentInChildren<Image>().sprite = recipe.icon;

            CraftingRecipe thisRecipe = recipe;

            go.GetComponent<Button>().onClick.AddListener(() => SelectRecipe(thisRecipe));
        }
    }

    void SelectRecipe(CraftingRecipe recipe)    // 특정 레시피를 선택하고 해당 레시피의 상세 정보를 UI에 표시하는 메서드
    {
        selectedRecipe = recipe;                            // 현재 선택된 레시피를 저장

        iconImage.sprite = recipe.resultItem.itemImage;     // 결과 아이템의 아이콘과 이름을 UI에 표시
        itemNameText.text = recipe.resultItem.itemName;

        foreach (Transform child in requiredListParent)      // 기존에 표시된 필요 재료 슬롯들을 모두 제거
            Destroy(child.gameObject);

        for (int i = 0; i < recipe.requiredItems.Length; i++)   // 레시피에 필요한 모든 재료를 순회하며 UI에 표시
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
                itemImage.sprite = input.itemImage;

            Debug.Log($"[Crafting] HasRequiredItems: {inventory.HasRequiredItems(recipe)} for {recipe.recipeName}");
        }

        craftButton.interactable = inventory.HasRequiredItems(recipe) && !isCrafting;  // 제작 버튼 활성화 여부를 설정, 필요한 재료를 모두 가지고 있고, 현재 제작 중이 아닐 때만 활성화
        ResetProgressBar();
    }

    void OnClickCraft()             // 제작 버튼 클릭 시 호출되는 메서드, 선택된 레시피로 아이템 제작을 시작
    {
        if (selectedRecipe == null) return;                          // 비어있을 때 작업 무시
        if (!inventory.HasRequiredItems(selectedRecipe)) return;     // 필요한 재료를 모두 가지고 있지 않으면 무시
        if (isCrafting) return;                                      // 이미 제작 중이면 무시

        // 제작 시작
        craftingCoroutine = StartCoroutine(CraftingProcess(selectedRecipe));     // 제작 프로세스 코루틴을 시작
    }

    IEnumerator CraftingProcess(CraftingRecipe recipe)          // 아이템 제작 과정을 처리하는 코루틴, 제작 시간 동안 진행 바를 업데이트하고, 완료 시 아이템을 지급
    {
        isCrafting = true;                      // 제작 중 상태로 설정하고 제작 버튼을 비활성화
        craftButton.interactable = false;

        float elapsed = 0f;         // 경과 시간과 총 제작 시간을 초기화
        float duration = recipe.craftDuration;

        while (elapsed < duration)  // 제작 시간이 완료될 때까지 반복
        {
            if (!go_CraftBase.activeSelf)   // 제작 UI가 닫혔는지 확인
            {
                // 창 닫혔으면 제작 중단
                StopCrafting();
                yield break;
            }

            elapsed += Time.deltaTime;          // 경과 시간을 업데이트
            progressBar.value = Mathf.Clamp01(elapsed / duration); // 진행 상태를 0~1 사이로 제한하여 시각적으로 표시

            yield return null;
        }

        // 제작 완료: 아이템 지급 처리
        inventory.CraftItem(recipe);    // 제작 완료: 인벤토리에 결과 아이템을 추가하고 재료 아이템을 제거
        SelectRecipe(recipe);   // UI를 갱신하여 변경된 인벤토리 상태를 반영

        isCrafting = false;     // 제작 상태를 초기화
        craftButton.interactable = inventory.HasRequiredItems(recipe);
        ResetProgressBar();     // 진행 바를 초기화

        craftingCoroutine = null;
    }

    void StopCrafting()     // 진행 중인 제작을 중단하는 메서드, // 제작 UI가 닫히거나 다른 이유로 제작을 중단해야 할 때 호출
    {
        if (craftingCoroutine != null)  // 현재 실행 중인 제작 코루틴이 있다면 중지
        {
            StopCoroutine(craftingCoroutine);
            craftingCoroutine = null;
        }
        isCrafting = false;         // 제작 중 상태를 해제
        craftButton.interactable = inventory.HasRequiredItems(selectedRecipe);       // 필요한 재료를 모두 가지고 있는 경우에만 제작 버튼을 다시 활성화
        ResetProgressBar();         // 진행 바를 초기화
        Debug.Log("제작 중단");
    }

    void ResetProgressBar()     // 제작 진행 바를 초기 상태(0)로 리셋하는 메서드
    {
        if (progressBar != null)
            progressBar.value = 0f;
    }
}