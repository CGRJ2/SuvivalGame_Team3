using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject go_inventoryBase;

    [SerializeField] private GameObject go_EquipmentSlotsParent;
    [SerializeField] private GameObject go_UsedSlotsParent;
    [SerializeField] private GameObject go_IngredientSlotsParent;
    [SerializeField] private GameObject go_FunctionSlotsParent;
    [SerializeField] private GameObject go_QuestSlotsParent;
    [SerializeField] private GameObject go_QuickSlotsParent;

    [SerializeField] private GameObject go_Base; // Tooltip Base_Outer

    private Slot[] equipmentSlots;
    private Slot[] consumableSlots;
    private Slot[] ingredientSlots;
    private Slot[] functionSlots;
    private Slot[] questSlots;
    private Slot[] quickSlots;

    private ItemType currentTab = ItemType.Equipment; // ±âº» ¹«±â ÅÇ

    private void Start()
    {
        equipmentSlots = go_EquipmentSlotsParent.GetComponentsInChildren<Slot>();
        consumableSlots = go_UsedSlotsParent.GetComponentsInChildren<Slot>();
        ingredientSlots = go_IngredientSlotsParent.GetComponentsInChildren<Slot>();
        functionSlots = go_FunctionSlotsParent.GetComponentsInChildren<Slot>();
        questSlots = go_QuestSlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = go_QuickSlotsParent.GetComponentsInChildren<Slot>();

        ChangeTab_UI(ItemType.Equipment); // ÃÊ±â ¹«±âÅÇ
    }


    public void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;

        if (inventoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        go_inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_inventoryBase.SetActive(false);
        go_Base.SetActive(false);
    }

    public void ChangeTab_UI(ItemType type)
    {
        currentTab = type;

        go_EquipmentSlotsParent.SetActive(false);
        go_UsedSlotsParent.SetActive(false);
        go_IngredientSlotsParent.SetActive(false);
        go_FunctionSlotsParent.SetActive(false);
        go_QuestSlotsParent.SetActive(false);

        switch (currentTab)
        {
            case ItemType.Equipment: go_EquipmentSlotsParent.SetActive(true); break;
            case ItemType.Used: go_UsedSlotsParent.SetActive(true); break;
            case ItemType.Ingredient: go_IngredientSlotsParent.SetActive(true); break;
            case ItemType.Function: go_FunctionSlotsParent.SetActive(true); break;
            case ItemType.Quest: go_QuestSlotsParent.SetActive(true); break;
        }
    }




    public void OnClickEquipmentTab() => ChangeTab_UI(ItemType.Equipment);
    public void OnClickUsedTab() => ChangeTab_UI(ItemType.Used);
    public void OnClickIngredient() => ChangeTab_UI(ItemType.Ingredient);
    public void OnClickFunctionTab() => ChangeTab_UI(ItemType.Function);
    public void OnClickQuestTab() => ChangeTab_UI(ItemType.Quest);
}
