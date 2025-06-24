using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStatus Status { get; private set; }
    public PlayerView View { get; private set; }
    public ColliderController Cc { get; private set; }

    public Vector3 InputDir { get; private set; }
    public Vector2 MouseInputDir { get; private set; }
    public Vector3 avatarForwardDir;
    private Vector2 currentRotation;

    [SerializeField] Transform[] TPS_Cameras;
    int currentZoomIndex = 1;


    private InputAction AimingAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction attackAction;
    private InputAction freeCamAction;

    //Vector3 moveDir;

    bool isMoveInput;
    bool isSprintInput;
    bool isJumpInput;
    bool isCrouchToggle;
    bool isAimingInput;
    bool isFreeCamModInput;
    bool isAttackInput;


    private void Awake() => Init();


    private void Update()
    {
        HandleSight();

        UpdateStateCondition();


    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void OnDestroy() => InputActionsDelete();

    private void Init()
    {
        Status = GetComponent<PlayerStatus>();
        View = GetComponent<PlayerView>();
        Cc = GetComponent<ColliderController>();

        Status.Init();
        InputActionsInit();
        StateMachineInit();
    }

    public void StateMachineInit()
    {
        Status.stateMachine.stateDic.Add(PlayerStateTypes.Idle, new Player_Idle(this));
        Status.stateMachine.stateDic.Add(PlayerStateTypes.Move, new Player_Move(this));
        Status.stateMachine.stateDic.Add(PlayerStateTypes.Sprint, new Player_Sprint(this));
        Status.stateMachine.stateDic.Add(PlayerStateTypes.Jump, new Player_Jump(this));
        Status.stateMachine.stateDic.Add(PlayerStateTypes.Fall, new Player_Fall(this));
        Status.stateMachine.stateDic.Add(PlayerStateTypes.Crouch, new Player_Crouch(this));
        Status.stateMachine.stateDic.Add(PlayerStateTypes.Attack, new Player_Attack(this));

        Status.stateMachine.CurState = Status.stateMachine.stateDic[PlayerStateTypes.Idle];
    }

    private void InputActionsInit()
    {
        // �÷��̾� ���� ��
        var playerControlMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");

        // ���� ī�޶�
        freeCamAction = playerControlMap.FindAction("FreeCamMod");
        freeCamAction.Enable();
        freeCamAction.started += HandleFreeCam;
        freeCamAction.canceled += HandleFreeCam;

        // ���� (��Ŀ��)
        AimingAction = playerControlMap.FindAction("Aiming");
        AimingAction.Enable();
        AimingAction.performed += HandleAiming;
        AimingAction.canceled += HandleAiming;


        // �޸��� �׼�
        sprintAction = playerControlMap.FindAction("Sprint");
        sprintAction.Enable();
        sprintAction.performed += HandleSprint;
        sprintAction.canceled += HandleSprint;

        // ���� �׼�
        jumpAction = playerControlMap.FindAction("Jump");
        jumpAction.Enable();
        jumpAction.performed += HandleJump;
        jumpAction.canceled += HandleJump;


        // �ɱ� �׼�
        crouchAction = playerControlMap.FindAction("Crouch");
        crouchAction.Enable();
        crouchAction.performed += HandleCrouch;
        crouchAction.canceled += HandleCrouch;

        // ���� �׼�
        attackAction = playerControlMap.FindAction("Attack");
        attackAction.Enable();
        attackAction.performed += HandleAttack;
        attackAction.canceled += HandleAttack;
    }

    private void InputActionsDelete()
    {
        freeCamAction.started -= HandleFreeCam;
        freeCamAction.canceled -= HandleFreeCam;

        AimingAction.performed -= HandleAiming;
        AimingAction.canceled -= HandleAiming;

        sprintAction.performed -= HandleSprint;
        sprintAction.canceled -= HandleSprint;

        jumpAction.performed -= HandleJump;
        jumpAction.canceled -= HandleJump;

        crouchAction.performed -= HandleCrouch;
        crouchAction.canceled -= HandleCrouch;

        attackAction.performed -= HandleAttack;
        attackAction.canceled -= HandleAttack;
    }


    public void HandleMove()
    {
        float moveSpeed;
        if (isSprintInput) moveSpeed = Status.SprintSpeed;
        else if (isCrouchToggle) moveSpeed = Status.CrouchSpeed;
        else moveSpeed = Status.MoveSpeed;

        Vector3 getMoveDir;

        if (isFreeCamModInput)
            getMoveDir = View.GetMoveDirection(InputDir,true);
        else
            getMoveDir = View.GetMoveDirection(InputDir);

        Vector3 moveVec = View.SetMove(getMoveDir, moveSpeed);
        View.moveDir = moveVec;
        View.facingDir = moveVec;

        if (InputDir != Vector3.zero)
        {
            View.animator.SetBool("IsMove", true);
            isMoveInput = true;
        }
        else
        {
            View.animator.SetBool("IsMove", false);
            isMoveInput = false;
        }
    }

    public void HandleSight()
    {
        Vector3 camRotateDir = View.SetAimRotation(MouseInputDir, Status.MinPitch, Status.MaxPitch);

        Vector3 avatarDir;
        if (isFreeCamModInput) avatarDir = View.facingDir;
        else if (!isMoveInput) avatarDir = camRotateDir; // ���� ��, ����ķ ��尡 �ƴ϶�� �ƹ�Ÿ�� �÷��̾��� ȭ���� ���� ����
        else avatarDir = View.moveDir;

        View.SetAvatarRotation(avatarDir, Status.RotateSpeed);

        // Aim ������ ����.
        if (isAimingInput)
        {
            View.animator.SetFloat("MoveX", InputDir.x);
            View.animator.SetFloat("MoveZ", InputDir.y);
        }
    }



    #region InputAction ó��
    public void OnMove(InputValue value)
    {
        InputDir = value.Get<Vector2>();
    }

    public void OnRotate(InputValue value)
    {
        Vector2 mouseDir = value.Get<Vector2>();
        mouseDir.y *= -1;
        MouseInputDir = mouseDir * Status.MouseSensitivity;
    }
    public void OnZoomInOut(InputValue value)
    {
        Vector2 scroll = value.Get<Vector2>();
        ZoomSet(scroll.y);
    }
    public void ZoomSet(float y)
    {
        if (y < 0)
        {
            if (currentZoomIndex < TPS_Cameras.Length - 1)
            {
                // ���� ī�޶� ����
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(false);

                // ���� �ܰ� ī�޶� �ѱ�
                currentZoomIndex++;
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(true);
            }
        }
        else if (y > 0)
        {
            if (currentZoomIndex > 0)
            {
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(false);

                currentZoomIndex--;
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(true);
            }
        }
    }

    public void HandleFreeCam(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isFreeCamModInput = true;
            View.FreeCamSet(true);
        }
            
        else if (context.canceled)
            isFreeCamModInput = false;
    }

    public void HandleAiming(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAimingInput = true;
            View.animator.SetBool("IsAiming", true);
        }
        else if (context.canceled)
        {
            isAimingInput = false;
            View.animator.SetBool("IsAiming", false);
        }
    }

    public void HandleSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            isSprintInput = true;
        else if (context.canceled)
            isSprintInput = false;
    }
    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            isJumpInput = true;
        if (context.canceled)
            isJumpInput = false;
    }

    public void HandleCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isCrouchToggle)
                isCrouchToggle = true;
            else
            {
                if (!Cc.GetIsHeadTouchedState())
                {
                    isCrouchToggle = false;
                }
            }
        }
            
    }

    public void HandleAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            isAttackInput = true;
        if (context.canceled)
            isAttackInput = false;
    }

    #endregion

    // InputFlag�鿡 ���� ���� ��ȯ �Ѱ�
    public void UpdateStateCondition()
    {
        // �ٴ� ���¶��
        if (Cc.GetIsGroundState())
        {
            // => Attack
            if (isAttackInput)
            {
                Debug.Log("Attack ���");
                Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Attack]);
            }
            // => Jump
            else if (isJumpInput && !isCrouchToggle)
            {
                Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Jump]);
            }
            // => Sprint
            else if (isSprintInput)
            {
                Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Sprint]);
            }
            // => Crouch
            else if (isCrouchToggle)
            {
                Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Crouch]);
            }
            // => Move
            else if (isMoveInput)
            {
                Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Move]);
            }
            // => Idle
            else
            {

                Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Idle]);
            }
        }
        else
        {
            Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Fall]);
        }
    }

    public void LoadPlayerData(PlayerStatus status)
    {

    }

    public void SavePlayerData(PlayerStatus status)
    {

    }
}
