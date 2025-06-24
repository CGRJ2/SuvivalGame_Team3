using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public float AttackCoolTime { get; private set; }
    public bool isAttacking;

    public PlayerStatus Status { get; private set; }
    public PlayerView View { get; private set; }
    public ColliderController Cc { get; private set; }

    public Vector3 InputDir { get; private set; }
    public Vector2 MouseInputDir { get; private set; }

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

    public Vector2 SmoothDir;      // 캐릭터가 실제로 쓸 방향
    public float SmoothTime = 0.1f; // 보간 속도
    private Vector2 smoothVelocity; // SmoothDamp용 내부 변수
    private void Update()
    {
        HandleSight();

        UpdateStateCondition();

        Status.stateMachine.Update();

        SmoothDir = Vector2.SmoothDamp(
            SmoothDir,
            InputDir,
            ref smoothVelocity,
            SmoothTime
        );
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
        // 플레이어 조작 맵
        var playerControlMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");

        // 자유 카메라
        freeCamAction = playerControlMap.FindAction("FreeCamMod");
        freeCamAction.Enable();
        freeCamAction.started += HandleFreeCam;
        freeCamAction.canceled += HandleFreeCam;

        /*// 조준 (포커싱)
        AimingAction = playerControlMap.FindAction("Aiming");
        AimingAction.Enable();
        AimingAction.performed += HandleAiming;
        AimingAction.canceled += HandleAiming;*/


        // 달리기 액션
        sprintAction = playerControlMap.FindAction("Sprint");
        sprintAction.Enable();
        sprintAction.performed += HandleSprint;
        sprintAction.canceled += HandleSprint;

        // 점프 액션
        jumpAction = playerControlMap.FindAction("Jump");
        jumpAction.Enable();
        jumpAction.performed += HandleJump;
        jumpAction.canceled += HandleJump;


        // 앉기 액션
        crouchAction = playerControlMap.FindAction("Crouch");
        crouchAction.Enable();
        crouchAction.performed += HandleCrouch;
        crouchAction.canceled += HandleCrouch;

        // 공격 액션
        attackAction = playerControlMap.FindAction("Attack");
        attackAction.Enable();
        attackAction.started += HandleAttack;
        attackAction.canceled += HandleAttack;
    }

    private void InputActionsDelete()
    {
        freeCamAction.started -= HandleFreeCam;
        freeCamAction.canceled -= HandleFreeCam;

        /*AimingAction.performed -= HandleAiming;
        AimingAction.canceled -= HandleAiming;*/

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
        if (isCrouchToggle) moveSpeed = Status.CrouchSpeed;
        else if (isSprintInput) moveSpeed = Status.SprintSpeed;
        else moveSpeed = Status.MoveSpeed;

        Vector3 getMoveDir;

        if (isFreeCamModInput)
            getMoveDir = View.GetMoveDirection(InputDir, true);
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
        // 프리캠 모드 => 플레이어의 이동 방향으로 아바타의 방향 맞춰주기
        if (isFreeCamModInput) avatarDir = View.facingDir;
        // 제 자리에 멈춰서서 프리캠 모드가 아니라면, 공격 도중이라면 =>  아바타가 플레이어의 화면을 향해 응시
        else if (!isMoveInput || IsCurrentState(PlayerStateTypes.Attack)) avatarDir = camRotateDir; 
        else avatarDir = View.moveDir;

        View.SetAvatarRotation(avatarDir, Status.RotateSpeed);

        // Attack 상태일 때만.
        if (Status.stateMachine.CurState == Status.stateMachine.stateDic[PlayerStateTypes.Attack])
        {
            View.animator.SetFloat("MoveX", SmoothDir.x);
            View.animator.SetFloat("MoveZ", SmoothDir.y);
        }
    }



    #region InputAction 처리
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
                // 현재 카메라 끄기
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(false);

                // 다음 단계 카메라 켜기
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

    /*public void HandleAiming(InputAction.CallbackContext context)
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
    }*/

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
        if (context.started)
            isAttackInput = true;
        if (context.canceled)
            isAttackInput = false;
    }

    #endregion

    public void CrouchToggleChange(bool value)
    {
        isCrouchToggle = value;
    }

    // InputFlag들에 따른 상태 전환 총괄
    public void UpdateStateCondition()
    {
        // 바닥 상태라면
        if (Cc.GetIsGroundState())
        {
            // => Attack 조건 : 입력값 존재 && 일반 or 기본이동 상태일 때만 가능
            if ( isAttackInput || isAttacking )
            {
                if ((IsCurrentState(PlayerStateTypes.Idle) || IsCurrentState(PlayerStateTypes.Move)))
                {
                    Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Attack]);
                }
                else if (IsCurrentState(PlayerStateTypes.Crouch))
                {
                    if (!Cc.GetIsHeadTouchedState())
                        Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Attack]);
                }
                else
                {
                    return;
                }
            }
            // => Jump
            else if (isJumpInput)
            {
                if (IsCurrentState(PlayerStateTypes.Attack))
                {
                    return;
                }
                else if (IsCurrentState(PlayerStateTypes.Crouch))
                {
                    if (!Cc.GetIsHeadTouchedState())
                        Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Jump]);
                }
                else
                {
                    Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Jump]);
                }
            }
            // => Sprint
            else if (isSprintInput)
            {
                if (IsCurrentState(PlayerStateTypes.Attack))
                {
                    return;
                }
                else if (IsCurrentState(PlayerStateTypes.Crouch))
                {
                    if (!Cc.GetIsHeadTouchedState())
                        Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Sprint]);
                }
                else
                {
                    Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Sprint]);
                }
            }
            // => Crouch
            else if (isCrouchToggle)
            {
                if (IsCurrentState(PlayerStateTypes.Attack)) return;
                else Status.stateMachine.ChangeState(Status.stateMachine.stateDic[PlayerStateTypes.Crouch]);
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

    public bool IsCurrentState(PlayerStateTypes state)
    {
        return Status.stateMachine.CurState == Status.stateMachine.stateDic[state];
    }

    public void Attack()
    {
        IDamagable[] damagables = Cc.GetDamagablesInRange();

        if (damagables == null) return;

        foreach (IDamagable damagable in damagables)
        {
            damagable.TakeDamage(Status.Damage);
        }
    }

   



    public void LoadPlayerData(PlayerStatus status)
    {

    }

    public void SavePlayerData(PlayerStatus status)
    {

    }
}
