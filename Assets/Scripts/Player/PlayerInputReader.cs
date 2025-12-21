using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)] // 컨트롤러보다 먼저 Update 되도록
public class PlayerInputReader : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    public PlayerInputData Data { get; private set; } = new PlayerInputData();

    private PlayerInputActions _actions;
    private PlayerInputActions.PlayerActions _playerMap;

    private void Awake()
    {
        _actions = new PlayerInputActions();
        _playerMap = _actions.Player;
        _playerMap.SetCallbacks(this);
    }

    private void OnEnable() => _actions.Enable();
    private void OnDisable() => _actions.Disable();

    // 컨트롤러 LateUpdate 에 한 번 호출해서 프레임 플래그 초기화
    public void EndFrame()
    {
        Data.ClearFrameFlags();
    }

    #region IPlayerActions 구현

    public void OnMove(InputAction.CallbackContext context)
    {
        // Move는 Value(Vector2)이니까 started/performed/canceled 모두에서 읽어도 됨
        Data.Move = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        Data.Rotate = context.ReadValue<Vector2>();
    }

    public void OnZoomInOut(InputAction.CallbackContext context)
    {
        // Scroll은 Vector2(보통 (0, delta)) 이라 Y축만 쓰는 식으로 많이 씀
        var v = context.ReadValue<Vector2>();
        Data.ZoomDelta += v.y;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Data.JumpPressed = true;
            Data.JumpHeld = true;
        }
        else if (context.canceled)
        {
            Data.JumpHeld = false;
            Data.JumpReleased = true;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Data.AttackPressed = true;
            Data.AttackHeld = true;
        }
        else if (context.canceled)
        {
            Data.AttackHeld = false;
            Data.AttackReleased = true;
        }
    }

    [SerializeField] private float rollTapThreshold = 0.2f; // 0.15~0.25 사이 적당히
    private float _sprintPressedTime;
    private bool _sprintIsHeld;
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _sprintPressedTime = Time.time;
            _sprintIsHeld = true;
            Data.SprintHeld = true;
        }
        else if (context.canceled)
        {
            float heldTime = Time.time - _sprintPressedTime;

            // 짧은 탭 => 구르기
            if (heldTime <= rollTapThreshold)
            {
                Data.RollPressed = true;
            }

            _sprintIsHeld = false;
            Data.SprintHeld = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            Data.CrouchHeld = context.ReadValueAsButton();
        else if (context.canceled)
            Data.CrouchHeld = false;
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        if (context.performed)
            Data.AimingHeld = context.ReadValueAsButton();
        else if (context.canceled)
            Data.AimingHeld = false;
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started)
            Data.RollPressed = true;
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started)
            Data.InteractionPressed = true;
    }

    public void OnFreeCamMod(InputAction.CallbackContext context)
    {
        // 필요하면 나중에 FreeCam용 Data에 추가
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.started)
            Data.InventoryPressed = true;
    }

    public void OnQuickSlots(InputAction.CallbackContext context)
    {
        // 어떤 키(1~4)가 눌렸는지 구분
        var controlPath = context.control.path; // "<Keyboard>/1"

        if (controlPath.EndsWith("/1"))
            Data.QuickSlotIndexPressed = 0;
        else if (controlPath.EndsWith("/2"))
            Data.QuickSlotIndexPressed = 1;
        else if (controlPath.EndsWith("/3"))
            Data.QuickSlotIndexPressed = 2;
        else if (controlPath.EndsWith("/4"))
            Data.QuickSlotIndexPressed = 3;
    }

    public void OnESC(InputAction.CallbackContext context)
    {
        if (context.started)
            Data.EscPressed = true;
    }

    #endregion
}
