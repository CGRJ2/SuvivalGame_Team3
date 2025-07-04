using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    PlayerManager pm;

    // Popup
    public PopUpUIGroup popUpUIGroup;

    // HUD
    public HUD_UIGroup hudGroup;

    // �κ��丮
    public InventoryUIGroup inventoryGroup;

    // ���۴�
    public CraftingUIGroup craftingGroup;

    // ���׷��̵�
    public UpgradeUIGroup upgradeGroup;





    // InputSystem => UI�׼Ǹ� ���� //////////////////////////////////////////////////////////
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    private InputAction escAction;
    private InputAction inventoryAction;
    /////////////////////////////////////////////////////////////////////////////////////////
    
    // ���� Ȱ��ȭ�� �г� ����
    Stack<GameObject> activedPanelStack = new Stack<GameObject>();

    public void Init()
    {
        base.SingletonInit();

    }

    private void Start()
    {
        pm = PlayerManager.Instance;
        playerActionMap = pm.instancePlayer.GetComponent<PlayerInput>().actions.FindActionMap("Player");
        uiActionMap = pm.instancePlayer.GetComponent<PlayerInput>().actions.FindActionMap("UI");

        escAction = uiActionMap.FindAction("Escape");
        escAction.performed += OnESC;
        
        inventoryAction = uiActionMap.FindAction("Inventory");
        inventoryAction.performed += OnInventory;
    }

    private void OnESC(InputAction.CallbackContext context)
    {
        if (activedPanelStack.Count > 0)
            ClosePanel();
        else { Debug.Log("�����ִ� �г��� ����"); }
    }
    
    private void OnInventory(InputAction.CallbackContext context)
    {
        // IŰ�ε� �κ��丮 ���� ����
        if (context.performed)
            UIManager.Instance.inventoryGroup.inventoryView.TryOpenInventory();
    }

    

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        activedPanelStack.Push(panel);
        playerActionMap.Disable();
        uiActionMap.Enable();
    }

    public void OpenPanelNotChangeActionMap(GameObject panel)
    {
        panel.SetActive(true);
        activedPanelStack.Push(panel);
    }

    public void ClosePanel()
    {
        if (activedPanelStack.Count < 1) return;


        activedPanelStack.Pop().SetActive(false);
        if (activedPanelStack.Count < 1)
        {
            Debug.Log("�г� �� �ݾ����ϱ� �÷��̾� �����δ�");
            playerActionMap.Enable();
            uiActionMap.Disable();
        }
    }

    public void CloseTargetPanel(GameObject target)
    {
        if (activedPanelStack.Count < 1) return;

        target.SetActive(false);

        // ��Ȱ��ȭ�� Ÿ�� �г� ���ÿ��� ���� �� ���� �����
        List<GameObject> tempList = new List<GameObject>(activedPanelStack);
        tempList.Remove(target);
        
        activedPanelStack = new Stack<GameObject>(tempList);

        if (activedPanelStack.Count < 1)
        {
            Debug.Log("�г� �� �ݾ����ϱ� �÷��̾� �����δ�");
            playerActionMap.Enable();
            uiActionMap.Disable();
        }
    }
}



