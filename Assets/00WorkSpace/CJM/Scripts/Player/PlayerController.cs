using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerView view;
    public Vector3 InputDir { get; private set; }
    public Vector2 MouseDir { get; private set; }
    public Vector3 avatarForwardDir;
    private Vector2 currentRotation;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity = 1;

    private InputAction freeCamAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction attackAction;

    //Vector3 moveDir;

    bool isSprintInput;
    bool isJumpInput;
    bool isAiming;


    private void Awake() => Init();


    private void Update()
    {
        HandleMove();
        /*view.SetAimRotation(MouseDir, minPitch, maxPitch);
        view.moveDir = view.GetMoveDirection(InputDir);
        status.movementState.Update();
        // ���� Ű �Է��� �ִٸ� ����
        if (isJumpInput)
        {
            status.movementState.ChangeState(status.movementState.stateDic[PlayerMovementStateTypes.Jump]);
        }
        // �̵� Ű �Է��� ���ٸ� ���� ����
        else if (view.moveDir != Vector3.zero)
        {
            status.movementState.ChangeState(status.movementState.stateDic[PlayerMovementStateTypes.Move]);
            view.animator.SetFloat("MoveX", InputDir.x);
            view.animator.SetFloat("MoveZ", InputDir.z);
        }

        // �ƹ��͵� �ش� ���ϸ� => Idle
        else
        {
            status.movementState.ChangeState(status.movementState.stateDic[PlayerMovementStateTypes.Idle]);
        }*/
    }

    private void FixedUpdate()
    {
        /*view.Move(status.MoveSpeed);
        view.SetAvatarRotation(status.RotateSpeed);*/
    }

    private void OnDestroy() => InputActionsDelete();

    private void Init()
    {
        InputActionsInit();
        StateMachineInit();
    }

    public void StateMachineInit()
    {
        status.movementState.stateDic.Add(PlayerMovementStateTypes.Idle, new Movement_Idle(view));
        status.movementState.stateDic.Add(PlayerMovementStateTypes.Move, new Movement_Move(view));
        status.movementState.stateDic.Add(PlayerMovementStateTypes.Sprint, new Movement_Sprint(view));
        status.movementState.stateDic.Add(PlayerMovementStateTypes.Jump, new Movement_Jump(view));
        status.movementState.stateDic.Add(PlayerMovementStateTypes.Fall, new Movement_Fall(view));

        status.movementState.CurState = status.movementState.stateDic[PlayerMovementStateTypes.Idle];
    }

    private void InputActionsInit()
    {
        // �÷��̾� ���� ��
        var playerControlMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");

        freeCamAction = playerControlMap.FindAction("FreeCamera");
        freeCamAction.Enable();
        freeCamAction.performed += HandleFreeCam;
        freeCamAction.canceled += HandleFreeCam;


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
        //crouchAction = playerControlMap.FindAction("Crouch");
        //crouchAction.Enable();
        //crouchAction.performed += HandleCrouch;
        //crouchAction.canceled += HandleCrouch;

        // ���� �׼�
        attackAction = playerControlMap.FindAction("Attack");
        attackAction.Enable();
        attackAction.started += HandleAttack;
        attackAction.canceled += HandleAttack;
    }

    private void InputActionsDelete()
    {
        freeCamAction.performed -= HandleFreeCam;
        freeCamAction.canceled -= HandleFreeCam;

        sprintAction.performed -= HandleSprint;
        sprintAction.canceled -= HandleSprint;

        jumpAction.performed -= HandleJump;
        jumpAction.canceled -= HandleJump;

        attackAction.started -= HandleAttack;
        attackAction.canceled -= HandleAttack;
    }





    public void OnMove(InputValue value)
    {
        InputDir = value.Get<Vector2>();
    }

    public void OnRotate(InputValue value)
    {
        Vector2 mouseDir = value.Get<Vector2>();
        mouseDir.y *= -1;
        MouseDir = mouseDir * mouseSensitivity;
    }

    public void HandleFreeCam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //view.FreeCamSet(true);
            view.isAiming = true;
            view.animator.SetBool("IsAiming", true);
        }
        else if (context.canceled)
        {
            //view.FreeCamSet(false);
            view.isAiming = false;
            view.animator.SetBool("IsAiming", false);
        }
    }

    public void HandleMove()
    {
        Vector3 camRotateDir = view.SetAimRotation(MouseDir, minPitch, maxPitch);

        float moveSpeed = status.MoveSpeed;

        Vector3 moveDir = view.SetMove(InputDir, moveSpeed);

        // �̰� ���ǵ鸸 ���� ���� �����������. 0623 ���� ����
        if (view.cc.isGrounded)
        {
            if (moveDir != Vector3.zero)
            {
                status.movementState.ChangeState(status.movementState.stateDic[PlayerMovementStateTypes.Move]);
            }
            else
            {
                status.movementState.ChangeState(status.movementState.stateDic[PlayerMovementStateTypes.Idle]);
            }
        }
        

        Vector3 avatarDir;
        if (view.isAiming) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        view.SetAvatarRotation(avatarDir, status.RotateSpeed);

        // Aim ������ ����.
        if (view.isAiming)
        {
            view.animator.SetFloat("MoveX", InputDir.x);
            view.animator.SetFloat("MoveZ", InputDir.y);
        }
    }

    public void HandleSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprintInput = true;
            view.animator.SetBool("IsSprint", true);
        }
        else if (context.canceled)
        {
            isSprintInput = false;
            view.animator.SetBool("IsSprint", false);
        }
    }
    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //isJumpCut = false;
            isJumpInput = true;
            view.Jump(status.JumpForce);
            view.animator.SetTrigger("Jump");
            // �ӽ� �׽�Ʈ �뵵

        }
        if (context.canceled)
        {
            //isJumpCut = true;
            isJumpInput = false;
        }
    }

    public void HandleAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            /*AttackDir = mouseWorldPos - (Vector2)transform.position;
            // ���� ����(&& Fall���� ����ó��) => �Ϲ� ����
            if (colliderState.isGrounded && stateMachine.CurState != stateMachine.stateDic[PlayerStateTypes.Fall])
            {
                if (stateMachine.CurState is Player_Attack attackState)
                {
                    attackState.AttackPlayByIndex();
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Attack]);
                }

            }*/
            // ���� ���� => ���� ����
            /*if (isJumping)
            {

            }*/
        }
    }
}
