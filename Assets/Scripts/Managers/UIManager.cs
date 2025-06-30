using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    public InventoryUI inventoryUI;

    public CraftingUI craftingUI;

    public void Init()
    {
        base.SingletonInit();

        
    }
}

[System.Serializable]
public class InventoryUI
{
    public InventoryView inventoryView;
    public SlotToolTip tooltip;
    public DragSlotView dragSlotInstance;
    public QuickSlotParent quickSlotParent;
}

[System.Serializable]
public class CraftingUI
{
    public CraftingUIGroup craftingUIGroup;
    

    // ���� �� ����
    public Image iconImage;                 // ���õ� �������� �������� ǥ���� Image ������Ʈ
    public Text itemNameText;               // ���õ� �������� �̸��� ǥ���� Text ������Ʈ
    public Transform requiredListParent;    // ���ۿ� �ʿ��� ��� ����� ǥ�õ� �θ� Transform
    public GameObject requiredSlotPrefab;   // �ʿ��� ��� ������ ǥ���� ���� ������
    public Button craftButton;              // ������ ������ �����ϴ� ��ư
    public Slider progressBar;              // ���� ���� ���¸� ǥ���ϴ� Slider ������Ʈ

    // ���� ���õ� ���� ������
    private Item_Recipe selectedRecipe;  

    // ���� ���� ����
    private Coroutine craftingCoroutine;    // ���� ���� ���� ���� �ڷ�ƾ�� ���� ����
    private bool isCrafting;                // ���� ���� ������ ���θ� ��Ÿ����

    

    public void PanelOpen()
    {
        craftingUIGroup.UpdateRecipePageData();
        craftingUIGroup.BasePanel.gameObject.SetActive(true);
    }


    
}
