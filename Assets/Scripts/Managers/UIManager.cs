using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;


public class UIManager : Singleton<UIManager>
{
    private Dictionary<Type, UIGroup> _groups = new Dictionary<Type, UIGroup>();

    // Popup
    public PopUpUIGroup popUpUIGroup => GetGroup<PopUpUIGroup>();
    // HUD
    public HUD_UIGroup hudGroup => GetGroup<HUD_UIGroup>();
    // 인벤토리
    public InventoryUIGroup inventoryGroup => GetGroup<InventoryUIGroup>();
    // 제작대
    public CraftingUIGroup craftingGroup => GetGroup<CraftingUIGroup>();
    // 업그레이드
    public UpgradeUIGroup upgradeGroup => GetGroup<UpgradeUIGroup>();


    // 그룹 등록
    public void RegisterGroup(UIGroup group)
    {
        var type = group.GetType();

        if (_groups.ContainsKey(type))
        {
            Debug.LogWarning($"[UIManager] 이미 등록된 UIGroup: {type.Name}");
            return;
        }

        _groups.Add(type, group);
    }

    // 타입으로 꺼내쓰기
    public T GetGroup<T>() where T : UIGroup
    {
        if (_groups.TryGetValue(typeof(T), out var group))
        {
            return group as T;
        }

        Debug.LogWarning($"[UIManager] 등록되지 않은 UIGroup 요청: {typeof(T).Name}");
        return null;
    }

    // 필요하면 생성까지 담당하는 헬퍼
    public T CreateGroup<T>() where T : UIGroup
    {
        // 이미 있으면 그대로 반환
        var exist = GetGroup<T>();
        if (exist != null)
            return exist;

        // 프리팹 로드
        var prefab = Resources.Load<GameObject>($"UIPrefabs/{typeof(T).Name}");
        if (prefab == null)
        {
            Debug.LogError($"[UIManager] 프리팹을 찾을 수 없습니다: UIPrefabs/{typeof(T).Name}");
            return null;
        }

        // 씬 내의 Canvas 자동 탐색
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("[UIManager] 씬에 Canvas가 없습니다. 새로 생성함.");
            //return null;
            canvas = Instantiate(Resources.Load<GameObject>($"UIPrefabs/Canvas").GetComponent<Canvas>());
        }

        // 캔버스 안에 해당 UI그룹 인스턴스 생성
        var go = Instantiate(prefab, canvas.transform);
        var group = go.GetComponent<T>();
        if (group == null)
        {
            Debug.LogError($"[UIManager] 프리팹에 {typeof(T).Name} 컴포넌트가 없습니다.");
            Destroy(go);
            return null;
        }

        RegisterGroup(group);
        return group;
    }


    // InputSystem => UI액션맵 정보 //////////////////////////////////////////////////////////
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    private InputAction escAction;
    private InputAction inventoryAction;
    /////////////////////////////////////////////////////////////////////////////////////////

    // 현재 활성화된 패널 스택
    Stack<GameObject> activedPanelStack = new Stack<GameObject>();

    private void Awake() => Init();

    public void Init()
    {
        base.SingletonInit();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CreateGroup<PopUpUIGroup>();
        CreateGroup<HUD_UIGroup>();
        CreateGroup<CraftingUIGroup>();
        CreateGroup<InventoryUIGroup>();
        CreateGroup<UpgradeUIGroup>();
    }

    private void Start()
    {
        PlayerManager pm = Manager.player;
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
        /*if (context.performed)
            UIManager.Instance.inventoryGroup.inventoryView.TryOpenInventory();*/
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

public class UIGroup : MonoBehaviour
{
   
}
