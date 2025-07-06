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

    // 인벤토리
    public InventoryUIGroup inventoryGroup;

    // 제작대
    public CraftingUIGroup craftingGroup;

    // 업그레이드
    public UpgradeUIGroup upgradeGroup;





    // InputSystem => UI액션맵 정보 //////////////////////////////////////////////////////////
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    private InputAction escAction;
    private InputAction inventoryAction;
    /////////////////////////////////////////////////////////////////////////////////////////

    // 현재 활성화된 패널 스택
    Stack<GameObject> activedPanelStack = new Stack<GameObject>();

    public void Init()
    {
        base.SingletonInit();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        else { Debug.Log("열려있는 패널이 없음"); }
    }
    
    private void OnInventory(InputAction.CallbackContext context)
    {
        // I키로도 인벤토리 끄기 가능
        if (context.performed)
            UIManager.Instance.inventoryGroup.inventoryView.TryOpenInventory();
    }

    public Stack<GameObject> GetActivedPanelStack()
    {
        return activedPanelStack;
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        activedPanelStack.Push(panel);
        playerActionMap.Disable();
        uiActionMap.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log($"현재 활성화된 패널 개수 {activedPanelStack.Count}");

    }

    public void OpenPanelNotChangeActionMap(GameObject panel)
    {
        panel.SetActive(true);
        activedPanelStack.Push(panel);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log($"현재 활성화된 패널 개수 {activedPanelStack.Count}");
    }

    public void ClosePanel()
    {
        if (activedPanelStack.Count < 1) return;


        activedPanelStack.Pop().SetActive(false);
        if (activedPanelStack.Count < 1)
        {
            Debug.Log("패널 다 닫았으니까 플레이어 움직인다");
            playerActionMap.Enable();
            uiActionMap.Disable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void CloseTargetPanel(GameObject target)
    {
        if (activedPanelStack.Count < 1) return;


        // 비활성화한 타겟 패널 스택에서 제거 후 스택 재생성
        List<GameObject> tempList = new List<GameObject>(activedPanelStack);
        tempList.Remove(target);
        target.SetActive(false);

        activedPanelStack = new Stack<GameObject>(tempList);

        if (activedPanelStack.Count < 1)
        {
            Debug.Log("패널 다 닫았으니까 플레이어 움직인다");
            playerActionMap.Enable();
            uiActionMap.Disable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}



